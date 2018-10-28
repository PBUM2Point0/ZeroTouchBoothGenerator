using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.FillingSpace.Lines
{
    public static class FL_Utils
    {
        /// <summary>
        /// Interprets lenghts as a line composed of the given lenghts
        /// and creates rectangles with the given width and an offset
        /// in the direction accross the line
        /// </summary>
        /// <param name="lengths">Lenghts to interpret as line</param>
        /// <param name="width">Width that the resulting rectangles wil have</param>
        /// <param name="direction">Direction of the line</param>
        /// <param name="accrossDirectionOffset">Offset of the line accross the line direction (default 0)</param>
        /// <param name="alongDirectionOffset">Offset of the line along the line direction (default 0)</param>
        /// <returns></returns>
        public static IEnumerable<FS_Rectangle> LineLengthsAsRectangles(
            IEnumerable<double> lengths,
            double width,
            FS_XYDirection direction,
            double accrossDirectionOffset = 0.0,
            double alongDirectionOffset = 0.0)
        {
            var currentPos = 0.0;
            foreach (var length in lengths)
            {
                yield return new FS_Rectangle(length, width,
                    new FS_Vector3(
                        direction.X * (currentPos + length / 2 + alongDirectionOffset) + direction.Y * (width / 2 + accrossDirectionOffset),
                        direction.Y * (currentPos + length / 2 + alongDirectionOffset) + direction.X * (width / 2 + accrossDirectionOffset), 0));

                currentPos += length;
            }
        }
    }
}
