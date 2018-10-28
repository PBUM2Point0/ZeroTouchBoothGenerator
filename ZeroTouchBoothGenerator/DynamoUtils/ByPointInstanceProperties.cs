using Autodesk.DesignScript.Geometry;
using Revit.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroTouchBoothGenerator.FillingSpace;

namespace ZeroTouchBoothGenerator.DynamoUtils
{
    public class ByPointInstanceProperties
    {
        public double Length { get; set; }
        public double Width { get; set; }
        public FamilyType Type { get; set; }
        public Point Position { get; set; }
        public double Rotation { get; set; }

        public ByPointInstanceProperties(
            double length,
            double width,
            FamilyType type,
            Point position,
            double rotation)
        {
            Length = length;
            Width = width;
            Type = type;
            Position = position;
            Rotation = rotation;
        }

        public static IEnumerable<ByPointInstanceProperties> CreateList(
            double length,
            double widht,
            FamilyType type,
            List<FS_Vector3> positions,
            double rotation)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                yield return new ByPointInstanceProperties(
                    length, widht, type,
                    Point.ByCoordinates(positions[i].X, positions[i].Y, positions[i].Z), rotation);
            }
        }

        public static IEnumerable<ByPointInstanceProperties> CreateList(
            List<double> lengths,
            List<double> widths,
            FamilyType type,
            List<FS_Vector3> positions,
            double rotation)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                yield return new ByPointInstanceProperties(
                    lengths[i], widths[i], type,
                    Point.ByCoordinates(positions[i].X, positions[i].Y, positions[i].Z), rotation);
            }
        }

        public static IEnumerable<ByPointInstanceProperties> CreateList(
           List<double> lengths,
           List<double> widths,
           List<FamilyType> type,
           List<FS_Vector3> positions,
           double rotation)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                yield return new ByPointInstanceProperties(
                    lengths[i], widths[i], type[i],
                    Point.ByCoordinates(positions[i].X, positions[i].Y, positions[i].Z), rotation);
            }
        }

        public static Dictionary<string, object> AsDictionary(IEnumerable<ByPointInstanceProperties> props)
        {
            return new Dictionary<string, object>()
                {
                    { "points", props.Select(p => p.Position).ToList() },
                    { "familyTypes", props.Select(p => p.Type).ToList() },
                    { "rotation", props.Select(p => p.Rotation).ToList() },
                    { "lengths", props.Select(p => p.Length).ToList() },
                    { "widths", props.Select(p => p.Width).ToList() }
                };
        }
    }
}
