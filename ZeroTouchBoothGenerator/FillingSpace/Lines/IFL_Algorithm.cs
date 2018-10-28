using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.FillingSpace.Lines
{
    public interface IFLAlgorithm
    {
        /// <summary>
        /// Calculates the length and number of parts to fill the given line and returns the lengths.
        /// </summary>
        /// <param name="line">Line to calculate the lenght and number of parts to be filled for</param>
        /// <returns>Lengths of parts that fill the given line.</returns>
        IEnumerable<double> FillLine(FL_Line line);
    }
}