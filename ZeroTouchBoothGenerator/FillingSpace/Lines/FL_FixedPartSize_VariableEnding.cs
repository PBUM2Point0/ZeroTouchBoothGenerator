using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.FillingSpace.Lines
{
    public class FL_FixedPartSize_VariableEnding : IFLAlgorithm
    {
        public double AllowedError { get; set; } = 0.0000001;
        public double PartWidth { get; set; }
        public bool EndPartAtStart { get; set; }
        public bool AlternateVariablePosition { get; set; }
        public double MinVariableSize { get; set; }


        public FL_FixedPartSize_VariableEnding(double partWidth)
        {
            PartWidth = partWidth;
        }

        public IEnumerable<double> FillLine(FL_Line line)
        {
            int numberOfFullParts = (int)(line.Length / PartWidth) - 1;

            yield return EndPartAtStart ?
                EndPartSize(line.Length) :
                StartPartSize(line.Length);

            for (int i = 0; i < numberOfFullParts; i++)
                yield return PartWidth;

            yield return !EndPartAtStart ?
                EndPartSize(line.Length) :
                StartPartSize(line.Length);

            if (AlternateVariablePosition)
                EndPartAtStart = !EndPartAtStart;
        }

        private double EndPartSize(double lineLength)
        {
            int numberOfFullParts = (int)(lineLength / PartWidth);
            double endPartLength = lineLength - (numberOfFullParts * PartWidth);

            if (endPartLength < MinVariableSize)
                return MinVariableSize;
            return endPartLength;
        }

        private double StartPartSize(double lineLength)
        {
            int numberOfFullParts = (int)(lineLength / PartWidth);
            double endPartLength = lineLength - (numberOfFullParts * PartWidth);

            if (endPartLength < MinVariableSize)
            {
                double difference = MinVariableSize - endPartLength;
                return PartWidth - difference;
            }
            else
            {
                return PartWidth;
            }
        }
    }
}
