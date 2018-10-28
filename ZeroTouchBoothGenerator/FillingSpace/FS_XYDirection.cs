using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.FillingSpace
{
    /// <summary>
    /// Determines wheter something is in direction X or Y
    /// only allowes for X or Y, not both or negative directions.
    /// </summary>
    public class FS_XYDirection
    {
        [IsVisibleInDynamoLibrary(false)]
        public double X { get; private set; }
        [IsVisibleInDynamoLibrary(false)]
        public double Y { get; private set; }


        [IsVisibleInDynamoLibrary(false)]
        public void SetXDirection() { X = 1.0; Y = 0.0; }
        [IsVisibleInDynamoLibrary(false)]
        public void SetYDirection() { X = 0.0; Y = 1.0; }

        public static FS_XYDirection XDirection => new FS_XYDirection() { X = 1.0 };
        public static FS_XYDirection YDirection => new FS_XYDirection() { Y = 1.0 };
    }
}
