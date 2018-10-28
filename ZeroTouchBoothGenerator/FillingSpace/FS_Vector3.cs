using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.FillingSpace
{
    [IsVisibleInDynamoLibrary(false)]
    public class FS_Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public FS_Vector3() { }

        public FS_Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool ValuesEqual(FS_Vector3 other, double delta)
        {
            return Math.Abs(other.X - X) < delta &&
                Math.Abs(other.Y - Y) < delta &&
                Math.Abs(other.Z - Z) < delta;
        }
    }
}
