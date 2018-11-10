using Autodesk.DesignScript.Geometry;
using Revit.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoUtils
{
    public class ByLineInstanceProperties
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public FamilyType Type { get; set; }
        public Level Level { get; set; }

        public ByLineInstanceProperties(
            Point p1,
            Point p2,
            FamilyType type,
            Level level)
        {
            P1 = p1;
            P2 = p2;
            Type = type;
            Level = level;
        }

        public static Dictionary<string, object> AsDictionary(IEnumerable<ByLineInstanceProperties> props)
        {
            return new Dictionary<string, object>()
                {
                    { "p1", props.Select(p => p.P1).ToList() },
                    { "p2", props.Select(p => p.P2).ToList() },
                    { "familytype", props.Select(p => p.Type).ToList() },
                    { "level", props.Select(p => p.Level).ToList() },
                };
        }
    }
}
