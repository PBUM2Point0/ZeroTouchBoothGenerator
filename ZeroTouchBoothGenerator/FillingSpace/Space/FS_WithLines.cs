using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroTouchBoothGenerator.FillingSpace.Lines;

namespace ZeroTouchBoothGenerator.FillingSpace.Space
{
    [IsVisibleInDynamoLibrary(false)]
    public class FS_WithLines : IFSAlgorithm
    {
        public IFLAlgorithm AlgoAlongLine { get; set; }
        public IFLAlgorithm AlgoAccrossLines { get; set; }

        public FS_WithLines(IFLAlgorithm algoAlongLine, IFLAlgorithm algoAccrossLines)
        {
            AlgoAlongLine = algoAlongLine;
            AlgoAccrossLines = algoAccrossLines;
        }


        /// <summary>
        /// Fills a space with the given algorithms. 
        /// Dierction indicates the direction of the lines.
        /// 
        /// E.g. 
        /// Direction X = 1 (Y = 0) <para/>
        /// ----- <para/>
        /// ----- <para/>
        /// Direction Y = 1 (X = 0) <para/>
        /// ||||| <para/>
        /// ||||| <para/>
        /// -> Length
        /// Direction
        /// </summary>
        /// <param name="totalLength"></param>
        /// <param name="totalWidth"></param>
        /// <returns></returns>
        public IEnumerable<IEnumerable<FS_Rectangle>> Fill(FS_LineFilledSpace space)
        {
            var lengthsAccrossLines = AlgoAccrossLines.FillLine(
                new FL_Line() { Length = space.LengthAlongLine });

            double currentPosAccross = 0;
            foreach (var lengthAccross in lengthsAccrossLines)
            {
                var lengthsAlongLine = AlgoAlongLine.FillLine(
                    new FL_Line() { Length = space.LengthAccrossLines });

                yield return FL_Utils.LineLengthsAsRectangles(lengthsAlongLine, lengthAccross, space.LineDirection, currentPosAccross).ToList();

                currentPosAccross += lengthAccross;
            }
        }
    }
}
