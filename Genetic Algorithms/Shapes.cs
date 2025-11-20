using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SideLib;
using PolygonLib;
using iShapeLib;
using iPolygonalLib;
using Point = PointLib.Point;
namespace ShapesLib
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

        public void Put(float x,float y)
        {
            this.Move(x - this.Center().Horisontal(), y - this.Center().Vertical());
        }

        public float Radius() { return radius; }

        public void Rotate(float degrees) { }

        public Circle Clone() {
            return new Circle(new Point (Center().Horisontal(),Center().Vertical()), Radius());
        }
    }

    public static class CrossShape
    {
        public static bool Cross(iShape shape1,iShape shape2)
        {
            return Cross((dynamic)shape1,(dynamic) shape2);
        }

        public static bool Cross(Circle shape1, Circle shape2)
        {
            return CrossShape.CrossCircles(shape1,shape2);
        }

        public static bool Cross(Polygon shape1, Polygon shape2)
        {
            return CrossShape.CrossPolygons(shape1, shape2);
        }

        public static bool Cross(Circle shape1, Polygon shape2)
        {
            return CrossShape.CrossCircleAndPolygon(shape2, shape1);
        }
        public static bool Cross(Polygon shape1, Circle shape2)
        {
            return CrossShape.CrossCircleAndPolygon(shape1, shape2);
        }

        public static float Line(float a, float b, float x)
        {
            return a * x + b;
        }

        public static (float a, float b) Parametres(Point point1, Point point2)
        {
            float a = ((point1.Vertical() - point2.Vertical()) / (point1.Horisontal() - point2.Horisontal()));
            float b = point1.Vertical() - point1.Horisontal() * a;
            return (a, b);
        }

        public static Point Intersection((float a, float b) line1, (float a, float b) line2)
        {
            float x = (line2.b - line1.b) / (line1.a - line2.a);
            float y = line1.a * x + line1.b;
            return new Point(x, y);
        }

        public static bool CrossCircles(Circle circle1,Circle circle2)
        {
            return circle1.Center().Distance(circle2.Center()) <= circle1.Radius() + circle2.Radius();
        }

        public static bool CrossPolygons(Polygon polygon1, Polygon polygon2)
        {
            foreach (Side side in polygon1.Sides())
                if (!ProjectionsOverlapPolygon(polygon1, polygon2, side))
                    return false;

            foreach (Side side in polygon2.Sides())
                if (!ProjectionsOverlapPolygon(polygon1, polygon2, side))
                    return false;

            return true;
        }

        public static bool CrossCircleAndPolygon(Polygon polygon, Circle circle)
        {
            foreach (Side side in polygon.Sides())
            {
                Point axis = new Point(-(side.SecondPoint().Vertical() - side.FirstPoint().Vertical()),
                                       side.SecondPoint().Horisontal() - side.FirstPoint().Horisontal());

                if (!ProjectionsOverlapCircle(polygon, circle, axis))
                    return false;
            }

            // ось от центра круга к ближайшей вершине полигона
            Point closest = polygon.Points().OrderBy(p => p.Distance(circle.Center())).First();
            Point axisToCircle = new Point(circle.Center().Horisontal() - closest.Horisontal(),
                                           circle.Center().Vertical() - closest.Vertical());

            if (!ProjectionsOverlapCircle(polygon, circle, axisToCircle))
                return false;

            return true;
        }

        private static bool ProjectionsOverlapPolygon(Polygon poly1, Polygon poly2, Side side)
        {
            Point axis = new Point(-(side.SecondPoint().Vertical() - side.FirstPoint().Vertical()),
                                   side.SecondPoint().Horisontal() - side.FirstPoint().Horisontal());

            float min1, max1, min2, max2;
            ProjectPolygon(poly1, axis, out min1, out max1);
            ProjectPolygon(poly2, axis, out min2, out max2);

            return !(max1 < min2 || max2 < min1);
        }

        private static bool ProjectionsOverlapCircle(Polygon polygon, Circle circle, Point axis)
        {
            float minPoly, maxPoly;
            ProjectPolygon(polygon, axis, out minPoly, out maxPoly);

            float length = (float)Math.Sqrt(axis.Horisontal() * axis.Horisontal() + axis.Vertical() * axis.Vertical());
            float centerProj = (circle.Center().Horisontal() * axis.Horisontal() + circle.Center().Vertical() * axis.Vertical()) / length;
            float minCircle = centerProj - circle.Radius();
            float maxCircle = centerProj + circle.Radius();

            return !(maxPoly < minCircle || maxCircle < minPoly);
        }

        private static void ProjectPolygon(Polygon polygon, Point axis, out float min, out float max)
        {
            float length = (float)Math.Sqrt(axis.Horisontal() * axis.Horisontal() + axis.Vertical() * axis.Vertical());
            min = max = (polygon.Points()[0].Horisontal() * axis.Horisontal() + polygon.Points()[0].Vertical() * axis.Vertical()) / length;

            foreach (Point p in polygon.Points())
            {
                float proj = (p.Horisontal() * axis.Horisontal() + p.Vertical() * axis.Vertical()) / length;
                if (proj < min) min = proj;
                if (proj > max) max = proj;
            }
        }


    }
}
