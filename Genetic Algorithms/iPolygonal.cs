using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SideLib;
using Point = PointLib.Point;

namespace iPolygonalLib
{
    public interface iPolygonal
    {
        public Side NearistSide(Point point);
        public float DistanceFromCenterToSide(Point point, Side side);
    }
}
