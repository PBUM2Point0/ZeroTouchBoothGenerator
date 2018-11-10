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
    public class FloorProperties
    {
        [IsVisibleInDynamoLibrary(false)]
        public ZeissBoothData BoothData { get; set; }

        [IsVisibleInDynamoLibrary(false)]
        public Level Level { get; set; }

        internal FloorProperties(
            ZeissBoothData boothData,
            Level level)
        {
            BoothData = boothData;
            Level = level;
        }

        /// <summary>
        /// Generates a new FloorProperties object
        /// </summary>
        /// <param name="data">ZeissBoothData</param>
        /// <param name="level">Level on which the floor is created</param>
        /// <returns></returns>
        public static FloorProperties ByData(
            ZeissBoothData data,
            Level level)
        {
            return new FloorProperties(
                data,
                level);
        }
    }

    public class FloorBorderProperties
    {
        [IsVisibleInDynamoLibrary(false)]
        public FloorProperties FloorProperties { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public Level Level { get; set; }

        internal FloorBorderProperties(
            FloorProperties floorProperties,
            FamilyType borderType,
            Level level)
        {
            FloorProperties = floorProperties;
            BorderType = borderType;
            Level = level;
        }

        public static FloorBorderProperties ByData(
            FloorProperties floorProperties,
            FamilyType borderType,
            Level level)
        {
            return new FloorBorderProperties(
                floorProperties,
                borderType,
                level);
        }

        [IsVisibleInDynamoLibrary(false)]
        public FamilyType BorderType { get; set; }
    }

    public class FlakeboardStripesProperties
    {
        [IsVisibleInDynamoLibrary(false)]
        public FloorProperties FloorData { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double FlakeboardDistanceToBounds { get; set; } = 0.1;
        [IsVisibleInDynamoLibrary(false)]
        public double DesiredAccrossFlakeboardDistance { get; set; } = 0.205;
        [IsVisibleInDynamoLibrary(false)]
        public double FlakeboardDistanceAlong { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public FamilyType FlakeboardType { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public Point FlakeBoardDirection { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double FlakeboardWidth { get; set; } = 0.1;
        [IsVisibleInDynamoLibrary(false)]
        public double FlakeboardLength { get; set; } = 2;


        //Calculated Properties
        [IsVisibleInDynamoLibrary(false)]
        public double LengthAccrossFlakeboards => FlakeBoardDirection.X > 0 ? FloorData.BoothData.LengthY : FloorData.BoothData.LengthX;
        [IsVisibleInDynamoLibrary(false)]
        public double LengthAlongFlakeboards => FlakeBoardDirection.X > 0 ? FloorData.BoothData.LengthX : FloorData.BoothData.LengthY;
        [IsVisibleInDynamoLibrary(false)]
        public double FlakeboardRotation => FlakeBoardDirection.X > 0 ? 0 : 90;

        internal FlakeboardStripesProperties() { }

        /// <summary>
        /// Creates a new FlakeboardStripesProperties object from the given data.
        /// </summary>
        /// <param name="floorData">FloorData object</param>
        /// <param name="flakeboardType">Flakeboard Type</param>
        /// <param name="flakeBoardDirection">Flakeboard Direction</param>
        /// <param name="desiredAccrossFlakeboardDistance">Accross Flakeboard Distance</param>
        /// <param name="flakeboardDistanceToBounds">Distance to booth bounds</param>
        /// <param name="flakeboardDistanceAlong">Distance along plates</param>
        /// <param name="flakeboardWidth">Board width</param>
        /// <param name="flakeboardLength">Board length</param>
        /// <returns></returns>
        public static FlakeboardStripesProperties ByData(
            FloorProperties floorData,
            FamilyType flakeboardType,
            Point flakeBoardDirection,
            double desiredAccrossFlakeboardDistance,
            double flakeboardDistanceToBounds,
            double flakeboardDistanceAlong,
            double flakeboardLength
            )
        {
            double width = (double)Parameter.ParameterByName(flakeboardType, "Breite").Value;

            return new FlakeboardStripesProperties()
            {
                FloorData = floorData,
                FlakeBoardDirection = flakeBoardDirection,
                DesiredAccrossFlakeboardDistance = desiredAccrossFlakeboardDistance,
                FlakeboardDistanceToBounds = flakeboardDistanceToBounds,
                FlakeboardDistanceAlong = flakeboardDistanceAlong,
                FlakeboardType = flakeboardType,
                FlakeboardWidth = width,
                FlakeboardLength = flakeboardLength
            };
        }

        public override string ToString()
        {
            return
                "Accross distance: " + DesiredAccrossFlakeboardDistance + "\t" +
                "Distance to bounds: " + FlakeboardDistanceToBounds + "\t" +
                "Distance along: " + FlakeboardDistanceAlong + "\t" +
                "Board width: " + FlakeboardWidth + "\t" +
                "Board Length: " + FlakeboardLength;
        }
    }

    public class FlakeboardPlatesProperties
    {
        [IsVisibleInDynamoLibrary(false)]
        public FloorProperties FloorData { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double DistanceToBorder { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public FamilyType FlakeboardType { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public Point LineDirection { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double PlateLength { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double MinimumPlateLength { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double PlateWidth { get; set; }
        [IsVisibleInDynamoLibrary(false)]
        public double ZOffset { get; set; } = 0;


        //Calculated Properties
        [IsVisibleInDynamoLibrary(false)]
        public FS_LineFilledSpace Space => new FS_LineFilledSpace(
            FloorData.BoothData.LengthX - 2 * DistanceToBorder,
            FloorData.BoothData.LengthY - 2 * DistanceToBorder,
            LineDirection);

        [IsVisibleInDynamoLibrary(false)]
        public double PlateRotation => IsDirectionX ? 0 : 90;
        [IsVisibleInDynamoLibrary(false)]
        public bool IsDirectionX => LineDirection.X > 0;
        [IsVisibleInDynamoLibrary(false)]
        public double PlateLengthX => IsDirectionX ? PlateWidth : PlateLength;
        [IsVisibleInDynamoLibrary(false)]
        public double PlateLenghtY => IsDirectionX ? PlateLenghtY : PlateWidth;


        internal FlakeboardPlatesProperties(
            FloorProperties floorData,
            FamilyType flakeboardType,
            double distanceToBorder,
            Point direction,
            double plateLength,
            double minPlateLength,
            double plateWidth,
            double zOffset)
        {
            FloorData = floorData;
            DistanceToBorder = distanceToBorder;
            FlakeboardType = flakeboardType;
            LineDirection = direction;
            PlateLength = plateLength;
            MinimumPlateLength = minPlateLength;
            PlateWidth = plateWidth;
            ZOffset = zOffset;
        }

        public static FlakeboardPlatesProperties ByData(
            FloorProperties floorData,
            FamilyType flakeboardType,
            double distanceToBorder,
            Point direction,
            double plateLength,
            double minPlateLength,
            double plateWidth,
            double zOffset)
        {
            return new FlakeboardPlatesProperties(
                floorData,
                flakeboardType,
                distanceToBorder,
                direction,
                plateLength,
                minPlateLength,
                plateWidth,
                zOffset);
        }
    }
}
