using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.FillingSpace
{
    [IsVisibleInDynamoLibrary(false)]
    public class FS_Rectangle
    {
        public double Length { get; set; }
        public double Width { get; set; }
        public FS_Vector3 CenterPosition { get; set; }

        public FS_Rectangle(double length, double width, FS_Vector3 centerPosition)
        {
            Length = length;
            Width = width;
            CenterPosition = centerPosition;
        }
    }
}
