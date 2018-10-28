using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Revit.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.BoothGen
{
    public class ZeissBoothData
    {
        //Booth
        [IsVisibleInDynamoLibrary(false)]
        public double LengthX { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double LengthY { get; set; }
        


        internal ZeissBoothData() { }

        /// <summary>
        /// Creates a new datatype holding all the data needed to build a 
        /// Zeiss boots
        /// </summary>
        /// <param name="lengthX">Length of the booth in x direction</param>
        /// <param name="lengthY">Lenght of the booth in y direction</param>
        /// <returns></returns>
        public static ZeissBoothData ByData(
            double lengthX,
            double lengthY
            )
        {
            return new ZeissBoothData()
            {
                LengthX = lengthX,
                LengthY = lengthY
            };
        }
    }
}
