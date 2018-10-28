using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.FillingSpace.Lines
{
    public class FL_FixedPartSize_VariableSpacing : IFLAlgorithm
    {
        public double DesiredSpace { get; set; }
        public double PartWidth { get; set; }
        public bool StartsWithSpace { get; set; }

        public FL_FixedPartSize_VariableSpacing(double desiredSpace, double partWidth, bool startsWithSpace)
        {
            StartsWithSpace = startsWithSpace;
            DesiredSpace = desiredSpace;
            PartWidth = partWidth;
        }

        public IEnumerable<double> FillLine(FL_Line line)
        {
            int numberOfParts = (int)(line.Length / (PartWidth + DesiredSpace) + 1);
            double lengthWithoutParts = line.Length - (numberOfParts * PartWidth);
            double spaceWidth = lengthWithoutParts / (numberOfParts - 1);


            bool spacePart = StartsWithSpace;
            for (int i = 0; i < numberOfParts + numberOfParts - 1; i++)
            {
                if (spacePart)
                    yield return spaceWidth;
                else
                    yield return PartWidth;

                spacePart = !spacePart;
            }
        }
    }
}
