using Autodesk.DesignScript.Geometry;
using Revit.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroTouchBoothGenerator.DynamoUtils
{
    public static class FamilyHelper
    {
        public static FamilyInstance Create(FamilyType type, Level level, Point position, double rotation, double length, double width)
        {
            FamilyInstance instance = FamilyInstance.ByPointAndLevel(type, position, level);
            instance.SetRotation(rotation);

            if (instance.Parameters.ToList().Exists(param => param.Name.Equals("Länge")))
            {
                instance.SetParameterByName("Länge", length);
            }

            if (instance.Parameters.ToList().Exists(param => param.Name.Equals("Breite")))
            {
                instance.SetParameterByName("Breite", width);
            }

            return instance;
        }
    }
}
