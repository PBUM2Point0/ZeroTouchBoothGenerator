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

        //Roof
        [IsVisibleInDynamoLibrary(false)]
        public int NumberOfRoofSpaces { get; set; }
        /// <summary>
        /// Direction of the spaces between the roof.
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public Point RoofDirection { get; set; }

        //MZ5
        [IsVisibleInDynamoLibrary(false)]
        public double MinimumMZ5Size { get; set; }


        //Blends
        [IsVisibleInDynamoLibrary(false)]
        public double MinimumBlendWidth { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public FamilyType[] BlendTypes { get; set; }


        //Constants
        [IsVisibleInDynamoLibrary(false)]
        public double MZ5Width => 0.08;
        [IsVisibleInDynamoLibrary(false)]
        public double BlendWidth => 0.3;
        [IsVisibleInDynamoLibrary(false)]
        public double BlendThickness => 0.019;
        [IsVisibleInDynamoLibrary(false)]
        public double StandardBlendWidth => 2;
        [IsVisibleInDynamoLibrary(false)]
        public double GapBetweenMZ5AndBlend => 0.001;




        //Calculated data
        [IsVisibleInDynamoLibrary(false)]
        public double LengthInSpaceDirection { get { return RoofDirection.X > 0 ? LengthX : LengthY; } }
        [IsVisibleInDynamoLibrary(false)]
        public double BoothLengthAcrossSpaces { get { return RoofDirection.X > 0 ? LengthY : LengthX; } }

        internal ZeissBoothData() { }

        /// <summary>
        /// Creates a new datatype holding all the data needed to build a 
        /// Zeiss boots
        /// </summary>
        /// <param name="lengthX">Length of the booth in x direction</param>
        /// <param name="lengthY">Lenght of the booth in y direction</param>
        /// <param name="numberOfRoofSpaces">Number of free spaces in the roof</param>
        /// <param name="roofDirection">Direction of the spaces in the roof x=1 => X direction, others must be zero.</param>
        /// <returns></returns>
        public static ZeissBoothData ByData(
            double lengthX,
            double lengthY,
            int numberOfRoofSpaces,
            Point roofDirection,
            double minimumMZ5Size,
            double minimumBlendenSize,
            FamilyType[] blendTypes
            )
        {
            return new ZeissBoothData()
            {
                LengthX = lengthX,
                LengthY = lengthY,
                NumberOfRoofSpaces = numberOfRoofSpaces,
                RoofDirection = roofDirection,
                MinimumMZ5Size = minimumMZ5Size,
                MinimumBlendWidth = minimumBlendenSize,
                BlendTypes = blendTypes,
            };
        }
    }
}
