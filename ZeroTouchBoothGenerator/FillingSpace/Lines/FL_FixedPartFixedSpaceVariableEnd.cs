using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.FillingSpace.Lines
{
    public class FL_FixedPartFixedSpaceVariableEnd : IFLAlgorithm
    {
        [IsVisibleInDynamoLibrary(false)]
        public double PartLength { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double SpaceLength { get; set; }

        [IsVisibleInDynamoLibrary(false)]
        public bool StartsWithSpace { get; set; } = true;
        [IsVisibleInDynamoLibrary(false)]
        public bool EndsWithSpace { get; set; }

        internal FL_FixedPartFixedSpaceVariableEnd(
            double partLength,
            double spaceLength,
            bool startWithSpace,
            bool endsWithSpace)
        {
            PartLength = partLength;
            SpaceLength = spaceLength;
            StartsWithSpace = startWithSpace;
            EndsWithSpace = endsWithSpace;
        }

        /// <summary>
        /// Creates an algorithm to fill a line alternating between, a line length
        /// and space length.
        /// If space and part length do not add up to the total length, the last 
        /// part is of variable size.
        /// Example:
        /// |S| P |S| P |S| P|
        /// </summary>
        /// <param name="partLength"></param>
        /// <param name="spaceLength"></param>
        /// <param name="startWithSpace"></param>
        /// <param name="endsWithSpace"></param>
        /// <returns></returns>
        public static FL_FixedPartFixedSpaceVariableEnd Create(
            double partLength,
            double spaceLength,
            bool startWithSpace,
            bool endsWithSpace)
        {
            return new FL_FixedPartFixedSpaceVariableEnd(
                partLength,
                spaceLength,
                startWithSpace,
                endsWithSpace);
        }

        public IEnumerable<double> FillLine(FL_Line line)
        {
            double currentRemaining = EndsWithSpace ? line.Length - SpaceLength : line.Length;
            bool nextIsSpace = StartsWithSpace;

            while (currentRemaining > 0)
            {
                if (nextIsSpace)
                {
                    currentRemaining -= SpaceLength;
                    yield return SpaceLength;
                }
                else if (currentRemaining - PartLength < 0)
                {
                    yield return currentRemaining;
                    currentRemaining = 0;
                }
                else
                {
                    currentRemaining -= PartLength;
                    yield return PartLength;
                }

                nextIsSpace = !nextIsSpace;
            }

            if (EndsWithSpace)
                yield return SpaceLength;
        }
    }
}
