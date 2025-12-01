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
using CircleLib;
using Point = PointLib.Point;
namespace CrossShapesLib
{
    public static class CrossShape
    {
        public static float Cross(iShape shape1,iShape shape2)
        {
            return Cross((dynamic)shape1,(dynamic) shape2);
        }

        public static float Cross(Circle shape1, Circle shape2)
        {
            return CrossShape.CircleIntersectionArea(shape1,shape2);
        }

        public static float Cross(Polygon shape1, Polygon shape2)
        {
            return CrossShape.PolygonPolygonIntersectionArea(shape1, shape2);
        }

        public static float Cross(Circle shape1, Polygon shape2)
        {
            return CrossShape.CirclePolygonIntersectionArea(shape1, shape2);
        }
        public static float Cross(Polygon shape1, Circle shape2)
        {
            return CrossShape.CirclePolygonIntersectionArea(shape2, shape1);
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

        public static float CircleIntersectionArea(Circle c1, Circle c2)
        {
            float r1 = c1.Radius();
            float r2 = c2.Radius();
            float d = c1.Center().Distance(c2.Center());

            if (d >= r1 + r2)
                return 0; // нет пересечения

            if (d <= Math.Abs(r1 - r2))
            {
                // один круг полностью внутри другого
                float smallerRadius = Math.Min(r1, r2);
                return (float)(Math.PI * smallerRadius * smallerRadius);
            }

            float r1Sq = r1 * r1;
            float r2Sq = r2 * r2;

            float alpha = (float)Math.Acos((d * d + r1Sq - r2Sq) / (2 * d * r1));
            float beta = (float)Math.Acos((d * d + r2Sq - r1Sq) / (2 * d * r2));

            float area = alpha * r1Sq + beta * r2Sq - 0.5f * (float)Math.Sqrt((-d + r1 + r2) * (d + r1 - r2) * (d - r1 + r2) * (d + r1 + r2));
            return area;
        }

        static float PolygonPolygonIntersectionArea(Polygon poly1, Polygon poly2)
        {
            var intersection = SutherlandHodgman(poly1.Points().ToList(), poly2.Points().ToList());
            return PolygonArea(intersection);
        }

        public static List<Point> SutherlandHodgman(List<Point> subject, List<Point> clip)
        {
            if (subject.Count == 0 || clip.Count == 0)
                return new List<Point>();

            List<Point> output = new List<Point>(subject);
            for (int i = 0; i < clip.Count; i++)
            {
                Point cp1 = clip[i];
                Point cp2 = clip[(i + 1) % clip.Count];
                List<Point> input = new List<Point>(output);
                output.Clear();

                if (input.Count == 0) break;

                Point s = input[input.Count - 1];
                foreach (Point e in input)
                {
                    if (Inside(e, cp1, cp2))
                    {
                        if (!Inside(s, cp1, cp2))
                            output.Add(Intersection(s, e, cp1, cp2));
                        output.Add(e);
                    }
                    else if (Inside(s, cp1, cp2))
                    {
                        output.Add(Intersection(s, e, cp1, cp2));
                    }
                    s = e;
                }
            }
            return output;
        }


        private static bool Inside(Point p, Point cp1, Point cp2)
        {
            return (cp2.Horisontal() - cp1.Horisontal()) * (p.Vertical() - cp1.Vertical()) - (cp2.Vertical() - cp1.Vertical()) * (p.Horisontal() - cp1.Horisontal()) >= 0;
        }

        private static Point Intersection(Point s, Point e, Point cp1, Point cp2)
        {
            float A1 = e.Vertical() - s.Vertical();
            float B1 = s.Horisontal() - e.Horisontal();
            float C1 = A1 * s.Horisontal() + B1 * s.Vertical();

            float A2 = cp2.Vertical() - cp1.Vertical();
            float B2 = cp1.Horisontal() - cp2.Horisontal();
            float C2 = A2 * cp1.Horisontal() + B2 * cp1.Vertical();

            float det = A1 * B2 - A2 * B1;
            if (Math.Abs(det) < 1e-6) return new Point(0, 0);

            float x = (B2 * C1 - B1 * C2) / det;
            float y = (A1 * C2 - A2 * C1) / det;
            return new Point(x, y);
        }

        static float CirclePolygonIntersectionArea(Circle circle, Polygon polygon)
        {
            float minX = Math.Min(polygon.Points().Min(p => p.Horisontal()), circle.Center().Horisontal() - circle.Radius());
            float maxX = Math.Max(polygon.Points().Max(p => p.Horisontal()), circle.Center().Horisontal() + circle.Radius());
            float minY = Math.Min(polygon.Points().Min(p => p.Vertical()), circle.Center().Vertical() - circle.Radius());
            float maxY = Math.Max(polygon.Points().Max(p => p.Vertical()), circle.Center().Vertical() + circle.Radius());

            float area = 0f;
            int steps = 200;
            float dx = (maxX - minX) / steps;
            float dy = (maxY - minY) / steps;

            for (float x = minX; x <= maxX; x += dx)
            {
                for (float y = minY; y <= maxY; y += dy)
                {
                    var p = new Point(x, y);
                    if (PointInsidePolygon(p, polygon) && circle.Center().Distance(p) <= circle.Radius())
                        area += dx * dy;
                }
            }
            return area;
        }

        static bool PointInsidePolygon(Point p, Polygon polygon)
        {
            bool inside = false;
            var points = polygon.Points();
            int j = points.Length - 1;
            for (int i = 0; i < points.Length; i++)
            {
                if ((points[i].Vertical() > p.Vertical()) != (points[j].Vertical() > p.Vertical()) &&
                    (p.Horisontal() < (points[j].Horisontal() - points[i].Horisontal()) * (p.Vertical() - points[i].Vertical()) / (points[j].Vertical() - points[i].Vertical()) + points[i].Horisontal()))
                {
                    inside = !inside;
                }
                j = i;
            }
            return inside;
        }

        static float PolygonArea(List<Point> points)
        {
            float area = 0;
            int j = points.Count - 1;
            for (int i = 0; i < points.Count; i++)
            {
                area += (points[j].Horisontal() + points[i].Horisontal()) * (points[j].Vertical() - points[i].Vertical());
                j = i;
            }
            return Math.Abs(area / 2f);
        }



    }
}
