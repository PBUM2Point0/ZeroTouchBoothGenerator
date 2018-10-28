using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.FillingSpace.Lines
{
    public class FL_FixedPartSize_EndingSameVarSize : IFLAlgorithm
    {
        public double AllowedError { get; set; } = 0.0000001;
        public double PartWidth { get; set; }
        public FL_FixedPartSize_EndingSameVarSize(double partWidth)
        {
            PartWidth = partWidth;
        }

        public IEnumerable<double> FillLine(FL_Line line)
        {
            int numberOfFullParts = (int)(line.Length / PartWidth);

            if ((line.Length - (numberOfFullParts * PartWidth) / 2) > AllowedError)
                yield return (line.Length - (numberOfFullParts * PartWidth)) / 2;

            for (int i = 0; i < numberOfFullParts; i++)
                yield return PartWidth;

            if ((line.Length - (numberOfFullParts * PartWidth)) / 2 > AllowedError)
                yield return (line.Length - (numberOfFullParts * PartWidth)) / 2;
        }
    }
}
