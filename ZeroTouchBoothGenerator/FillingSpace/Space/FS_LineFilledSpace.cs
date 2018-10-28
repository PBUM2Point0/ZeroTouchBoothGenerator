using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.FillingSpace.Space
{
    /// A space with Length and width.
    /// Dierction indicates the direction of the lines.
    /// 
    /// E.g. 
    /// Direction X = 1 (Y = 0)
    /// -----
    /// -----
    /// Direction Y = 1 (X = 0)
    /// |||||
    /// |||||
    /// -> Length
    [IsVisibleInDynamoLibrary(false)]
    public class FS_LineFilledSpace
    {
        [IsVisibleInDynamoLibrary(false)]
        public double Length { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double Width { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public FS_XYDirection LineDirection { get; set; }

        [IsVisibleInDynamoLibrary(false)]
        public double LengthAlongLine => LineDirection.X * Width + LineDirection.Y * Length;
        [IsVisibleInDynamoLibrary(false)]
        public double LengthAccrossLines => LineDirection.X * Length + LineDirection.Y * Width;

        [IsVisibleInDynamoLibrary(false)]
        public FS_LineFilledSpace(double length, double width, FS_XYDirection lineDirection)
        {
            Length = length;
            Width = width;
            LineDirection = lineDirection;
        }

        [IsVisibleInDynamoLibrary(false)]
        public FS_LineFilledSpace(double length, double width, Point lineDirection)
        {
            Length = length;
            Width = width;
            LineDirection = lineDirection.X > 0 ? FS_XYDirection.XDirection : FS_XYDirection.YDirection;
        }
    }
}
