using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Revit.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroTouchBoothGenerator.DynamoUtils;
using ZeroTouchBoothGenerator.FillingSpace;
using ZeroTouchBoothGenerator.FillingSpace.Lines;
using ZeroTouchBoothGenerator.FillingSpace.Space;

namespace ZeroTouchBoothGenerator.BoothGen
{
    public static class FloorGenerator
    {
        [MultiReturn(new[] { "points", "familyTypes", "rotation", "lengths", "widths" })]
        public static Dictionary<string, object> CreateFlakeboardStripes(FlakeboardStripesProperties props)
        {
            FS_WithLines filler = new FS_WithLines(
                new FL_FixedPartFixedSpaceVariableEnd(
                    props.FlakeboardLength,
                    props.FlakeboardDistanceAlong,
                    true, true),
                new FL_FixedPartSize_VariableSpacing(
                    props.DesiredAccrossFlakeboardDistance,
                    props.FlakeboardWidth,
                    false));

            var result = filler.Fill(new FS_LineFilledSpace(
                    props.FloorData.BoothData.LengthX - 2 * props.FlakeboardDistanceToBounds,
                    props.FloorData.BoothData.LengthY,
                    props.FlakeBoardDirection));

            result = RemoveUnevenIndices(result);

            var allLines = new List<FS_Rectangle>();
            foreach (var line in result)
            {
                var filteredLines = RemoveEvenIndices(line);
                allLines.AddRange(filteredLines);
            }

            //Add Distance To Bound in AccrossDirection
            allLines.ForEach(r =>
            {
                r.CenterPosition.X += props.FlakeboardDistanceToBounds * props.FlakeBoardDirection.Y;
                r.CenterPosition.Y += props.FlakeboardDistanceToBounds * props.FlakeBoardDirection.X;
            });

            var instanceProps = ByPointInstanceProperties.CreateList(
                allLines.Select(l => l.Length).ToList(),
                allLines.Select(l => l.Width).ToList(),
                props.FlakeboardType,
                allLines.Select(l => l.CenterPosition).ToList(),
                props.FlakeboardRotation);


            return ByPointInstanceProperties.AsDictionary(instanceProps);
        }

        private static IEnumerable<T> RemoveEvenIndices<T>(IEnumerable<T> enumerable) => enumerable.Where((val, index) => index % 2 == 1);
        private static IEnumerable<T> RemoveUnevenIndices<T>(IEnumerable<T> enumerable) => enumerable.Where((val, index) => index % 2 == 0);

        [IsVisibleInDynamoLibrary(false)]
        public static IEnumerable<FamilyType> ToggleFamilyTypes(
            IEnumerable<double> lengths,
            FamilyType type1,
            FamilyType type2)
        {
            bool isFirst = true;
            foreach (var item in lengths)
            {
                FamilyType type = isFirst ? type1 : type2;

                yield return type;
                isFirst = !isFirst;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "p1", "p2", "familytype", "level" })]
        public static Dictionary<string, object> CreateBorder(FloorBorderProperties props)
        {
            //TODO: Uncomment
            var frontPoint = Point.ByCoordinates(0, 0, 0);
            var frontLength = props.FloorProperties.BoothData.LengthX;
            var frontInstanceProps = new ByLineInstanceProperties(
                    frontPoint,
                    Point.ByCoordinates(frontPoint.X + frontLength, frontPoint.Y, frontPoint.Z),
                    props.BorderType,
                    props.Level);

            var rightPoint = Point.ByCoordinates(frontLength, 0, 0);
            var rightLength = props.FloorProperties.BoothData.LengthY;
            var rightInstanceProps = new ByLineInstanceProperties(
                   rightPoint,
                   Point.ByCoordinates(rightPoint.X, rightPoint.Y + rightLength, rightPoint.Z),
                   props.BorderType,
                   props.Level);

            var backPoint = Point.ByCoordinates(frontLength, rightLength, 0);
            var backLength = frontLength;
            var backInstanceProps = new ByLineInstanceProperties(
                   backPoint,
                   Point.ByCoordinates(backPoint.X - frontLength, backPoint.Y, backPoint.Z),
                   props.BorderType,
                   props.Level);

            var leftPoint = Point.ByCoordinates(0, rightLength, 0);
            var leftLength = rightLength;
            var leftInstanceProps = new ByLineInstanceProperties(
                   leftPoint,
                   Point.ByCoordinates(leftPoint.X, leftPoint.Y - leftLength, leftPoint.Z),
                   props.BorderType,
                   props.Level);

            return ByLineInstanceProperties.AsDictionary(new List<ByLineInstanceProperties>()
            {
                frontInstanceProps,
                rightInstanceProps,
                backInstanceProps,
                leftInstanceProps,
            });
        }

        [MultiReturn(new[] { "points", "familyTypes", "rotation", "lengths", "widths" })]
        public static Dictionary<string, object> CreateFlakeboardPlates(FlakeboardPlatesProperties props)
        {
            var spaceFiller = new FS_WithLines(
                new FL_FixedPartSize_VariableEnding(props.PlateLength)
                {
                    AlternateVariablePosition = true,
                    MinVariableSize = props.MinimumPlateLength
                },
                new FL_FixedPartSize_VariableEnding(props.PlateWidth));

            var lines = spaceFiller.Fill(props.Space).ToList();


            var flattendedRectangles = lines.SelectMany(r => r).ToList();
            //Adjust centerPositions by distance to border
            flattendedRectangles.ForEach(r =>
            {
                r.CenterPosition.Y += props.DistanceToBorder;
                r.CenterPosition.X += props.DistanceToBorder;
            });
            var positions = flattendedRectangles.Select(r => r.CenterPosition).ToList();


            var allInstanceProperties = ByPointInstanceProperties.CreateList(
               flattendedRectangles.Select(r => r.Length).ToList(),
               flattendedRectangles.Select(r => r.Width).ToList(),
               props.FlakeboardType,
               positions,
               props.PlateRotation);

            return ByPointInstanceProperties.AsDictionary(allInstanceProperties);
        }

        [MultiReturn(new[] { "points", "familyTypes", "rotation", "lengths", "widths" })]
        public static Dictionary<string, object> CreateFloorPlates(FloorPlateProperties props)
        {
            //Berechne Offset-Positionen für jeden Space/WhiteLine
            //TODO: Siehe Beschreibung
            var positions = new List<Tuple<FamilyType, double>>();

            var currentDistance = 0.0;
            if (props.FirstLineIsWhite)
            {
                positions.Add(new Tuple<FamilyType, double>(props.WhitePlates, 0.0));
                currentDistance += props.WhitePlateWidth + props.GapWidthToNormalPlate;
            }

            for (int k = props.FirstLineIsWhite ? 1 : 0; k < props.DistancesBetweenWhiteLines.Count; k++)
            {
                positions.Add(new Tuple<FamilyType, double>(props.NormalPlates, currentDistance));
                currentDistance += props.DistancesBetweenWhiteLines[k] + props.GapWidthToNormalPlate;
                positions.Add(new Tuple<FamilyType, double>(props.WhitePlates, currentDistance));
                currentDistance += props.WhitePlateWidth + props.GapWidthToNormalPlate;
            }

            //Last normal width
            positions.Add(new Tuple<FamilyType, double>(props.NormalPlates, currentDistance));

            //Vorgehen: Erstelle Liste von Listen mit SFRectangles + Typ (Weiß/normal)
            var rectangleTypes = new List<Tuple<List<FS_Rectangle>, FamilyType>>();
            //TODO: Create Rectangles
            //int i = 0;
            //foreach (var pos in positions)
            //{
            //    if (pos.Item1 == props.WhitePlates)
            //    {
            //        var lineGenerator = new FLWithFixedPartSizeAndVariableEnding(props.WhitePlateLength);
            //        var lengths = lineGenerator.FillLine(new FLLine() { Length = props.Space.Length });

            //        var rectangles = FLUtils.LineLengthsAsRectangles(
            //            lengths,
            //            props.WhitePlateWidth,
            //            props.LineDirection.X > 0 ? FSXYDirection.XDirection : FSXYDirection.YDirection,
            //            pos.Item2,
            //            props.BorderThickness);

            //        rectangleTypes.Add(new Tuple<List<FSRectangle>, FamilyType>(rectangles.ToList(), props.WhitePlates));
            //    }
            //    else
            //    {
            //        var spaceGenerator = new FSWithLines(
            //            new FLWithFixedPartSizeAndVariableEnding(props.NormalPlateLength),
            //            new FLWithFixedPartSizeAndVariableEnding(props.NormalPlateWidth));

            //        var nextSpaceBegin = positions.Count > i + 1 ? positions[i + 1].Item2 : props.Space.LengthAccrossLines;
            //        var rectangles = spaceGenerator.Fill(props.Space).SelectMany(l => l).ToList();

            //        rectangleTypes.Add(new Tuple<List<FSRectangle>, FamilyType>(rectangles.ToList(), props.NormalPlates));
            //    }

            //    i++;
            //}

            //var instanceProps = ByPointInstanceProperties.CreateList(
            //    rectangleTypes.Select(l => l.Item1.Select(r => r.Length)).SelectMany(r1 => r1).ToList(),
            //    rectangleTypes.Select(l => l.Item1.Select(r => r.Width)).SelectMany(r1 => r1).ToList(),
            //    rectangleTypes.Select(l => l.Item2).ToList(),
            //    rectangleTypes.Select(l => l.Item1.Select(r => r.CenterPosition)).SelectMany(r1 => r1).ToList(),
            //    props.PlateRotation
            //    );

            //return ByPointInstanceProperties.AsDictionary(instanceProps);

            return null;
        }
    }
}
