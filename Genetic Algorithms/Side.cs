using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointLib;
using Point = PointLib.Point;
namespace SideLib
{
    public class Side
    {
        private Point point1, point2;
        public Side(Point point1, Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        public float Length()
        {
            return (float)Math.Sqrt(Math.Pow(point1.Horisontal() - point2.Horisontal(), 2) + Math.Pow(point1.Vertical() - point2.Vertical(), 2));
        }

        public void Move(float x, float y)
        {
            point1.Move(x, y);
            point2.Move(x, y);
        }

        public float Distance(Point point)
        {
            return point1.Distance(point) + point2.Distance(point);
        }

        public Point FirstPoint()
        {
            return point1;
        }

        public Point SecondPoint()
        {
            return point2;
        }
    }
}
