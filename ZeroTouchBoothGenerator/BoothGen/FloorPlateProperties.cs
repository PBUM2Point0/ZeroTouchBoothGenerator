using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Revit.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroTouchBoothGenerator.FillingSpace.Space;

namespace BoothGenerator
{
    public class FloorPlateProperties
    {
        [IsVisibleInDynamoLibrary(false)]
        public FloorProperties FloorProps { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public FamilyType WhitePlates { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public FamilyType NormalPlates { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public List<double> DistancesBetweenWhiteLines { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double GapWidthToNormalPlate { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public Point LineDirection { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double WhitePlateLength { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double WhitePlateWidth { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double NormalPlateLength { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double NormalPlateWidth { get; set; }
        /// <summary>
        /// Space between white and normal plates.
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public double BorderThickness { get; set; } = 0.003;
        [IsVisibleInDynamoLibrary(false)]
        public double PlateRotation => LineDirection.X > 0 ? 0 : 90;

        //Calculates Properties
        [IsVisibleInDynamoLibrary(false)]
        public FS_LineFilledSpace Space => new FS_LineFilledSpace(
            FloorProps.BoothData.LengthX - BorderThickness,
            FloorProps.BoothData.LengthY - BorderThickness,
            LineDirection);

        [IsVisibleInDynamoLibrary(false)]
        public bool FirstLineIsWhite => DistancesBetweenWhiteLines.Count > 0 && DistancesBetweenWhiteLines[0] == 0;


        internal FloorPlateProperties(
            FloorProperties floorProps,
            FamilyType whitePlates,
            FamilyType normalPlates,
            Point lineDirection,
            double whitePlateWidth,
            double whitePlateLength,
            double normalPlateWidth,
            double normalPlateLength,
            List<double> distancesBetweenWhiteLines,
            double gapWidthToNormalPlate)
        {
            FloorProps = floorProps;
            LineDirection = lineDirection;
            WhitePlates = whitePlates;
            NormalPlates = normalPlates;
            WhitePlateLength = whitePlateLength;
            WhitePlateWidth = WhitePlateWidth;
            NormalPlateWidth = normalPlateWidth;
            NormalPlateLength = normalPlateLength;
            DistancesBetweenWhiteLines = distancesBetweenWhiteLines;
            GapWidthToNormalPlate = gapWidthToNormalPlate;
        }

        public static FloorPlateProperties ByData(
            FloorProperties floorProps,
            FamilyType whitePlates,
            FamilyType normalPlates,
            Point lineDirection,
            double whitePlateWidth,
            double whitePlateLength,
            double normalPlateWidth,
            double normalPlateLength,
            List<double> distancesBetweenWhiteLines,
            double gapWidthToNormalPlate)
        {
            return new FloorPlateProperties(floorProps,
                whitePlates,
                normalPlates,
                lineDirection,
                whitePlateWidth,
                whitePlateLength,
                normalPlateWidth,
                normalPlateLength,
                distancesBetweenWhiteLines,
                gapWidthToNormalPlate);
        }
    }
}
