using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iShapeLib;
using Point = PointLib.Point;
namespace CircleLib
{
    public class Circle : iShape
    {
        private Point center;
        private float radius;

        public Circle(Point center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public void Move(float x, float y)
        {
            center.Move(x, y);
        }

        public Point Center()
        {
            return center;
        }

        public void Put(float x, float y)
        {
            this.Move(x - this.Center().Horisontal(), y - this.Center().Vertical());
        }

        public float Radius() { return radius; }

        public void Rotate(float degrees) { }

        public Circle Clone()
        {
            return new Circle(new Point(Center().Horisontal(), Center().Vertical()), Radius());
        }
    }
}
