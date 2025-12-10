using System;
using System.Collections.Generic;
using System.Linq;
using PointLib;
using PolygonLib;
using CircleLib;
using iShapeLib;
using Point = PointLib.Point;
namespace CrossShapesLib
{
    public static class CrossShape
    {
        public static float Cross(iShape s1, iShape s2) => Cross((dynamic)s1, (dynamic)s2);

        public static float Cross(Circle c1, Circle c2)
            => CircleIntersectionArea(c1, c2);

        public static float Cross(Polygon p1, Polygon p2)
            => PolygonPolygonIntersectionArea(p1, p2);

        public static float Cross(Circle c, Polygon p)
            => CirclePolygonIntersectionArea(c, p);

        public static float Cross(Polygon p, Circle c)
            => CirclePolygonIntersectionArea(c, p);

        public static float Line(float a, float b, float x) => a * x + b;

        public static (float a, float b) Parametres(Point p1, Point p2)
        {
            float a = (p1.Vertical() - p2.Vertical()) /
                      (p1.Horisontal() - p2.Horisontal());
            float b = p1.Vertical() - a * p1.Horisontal();
            return (a, b);
        }

        public static Point Intersection((float a, float b) L1, (float a, float b) L2)
        {
            float x = (L2.b - L1.b) / (L1.a - L2.a);
            return new Point(x, L1.a * x + L1.b);
        }

        
        public static float CircleIntersectionArea(Circle c1, Circle c2)
        {
            float r1 = c1.Radius();
            float r2 = c2.Radius();
            float d = c1.Center().Distance(c2.Center());

            if (d >= r1 + r2) return 0;

            if (d <= Math.Abs(r1 - r2))
                return MathF.PI * MathF.Min(r1, r2) * MathF.Min(r1, r2);

            float r1Sq = r1 * r1;
            float r2Sq = r2 * r2;

            float alpha = MathF.Acos((d * d + r1Sq - r2Sq) / (2 * d * r1));
            float beta = MathF.Acos((d * d + r2Sq - r1Sq) / (2 * d * r2));

            return r1Sq * alpha + r2Sq * beta -
                   0.5f * MathF.Sqrt(
                       (-d + r1 + r2) *
                       (d + r1 - r2) *
                       (d - r1 + r2) *
                       (d + r1 + r2));
        }

        
        static float PolygonPolygonIntersectionArea(Polygon a, Polygon b)
        {
            var inter = SutherlandHodgman(a.Points().ToList(), b.Points().ToList());
            if (inter.Count < 3) return 0;
            return PolygonArea(inter);
        }

        
        public static List<Point> SutherlandHodgman(List<Point> subject, List<Point> clip)
        {
            if (subject.Count == 0 || clip.Count == 0)
                return new List<Point>();

            List<Point> output = new List<Point>(subject);

            for (int i = 0; i < clip.Count; i++)
            {
                Point A = clip[i];
                Point B = clip[(i + 1) % clip.Count];

                var input = output;
                output = new List<Point>();

                if (input.Count == 0) break;

                Point prev = input[^1];

                foreach (var curr in input)
                {
                    bool currIn = Inside(curr, A, B);
                    bool prevIn = Inside(prev, A, B);

                    if (currIn)
                    {
                        if (!prevIn)
                            output.Add(LineIntersection(prev, curr, A, B));
                        output.Add(curr); 
                    }
                    else if (prevIn)
                        output.Add(LineIntersection(prev, curr, A, B)); 

                    prev = curr;
                }
            }
            return output;
        }

        private static bool Inside(Point p, Point a, Point b)
            => (b.Horisontal() - a.Horisontal()) * (p.Vertical() - a.Vertical())
             - (b.Vertical() - a.Vertical()) * (p.Horisontal() - a.Horisontal())
               >= 0;

        private static Point LineIntersection(Point s, Point e, Point a, Point b)
        {
            float A1 = e.Vertical() - s.Vertical();
            float B1 = s.Horisontal() - e.Horisontal();
            float C1 = A1 * s.Horisontal() + B1 * s.Vertical();

            float A2 = b.Vertical() - a.Vertical();
            float B2 = a.Horisontal() - b.Horisontal();
            float C2 = A2 * a.Horisontal() + B2 * a.Vertical();

            float det = A1 * B2 - A2 * B1;
            if (Math.Abs(det) < 1e-6) return s;

            float x = (B2 * C1 - B1 * C2) / det;
            float y = (A1 * C2 - A2 * C1) / det;
            return new Point(x, y);
        }

        static float CirclePolygonIntersectionArea(Circle circle, Polygon polygon)
        {
            Polygon circleApprox = ApproximateCircleAsPolygon(circle, sides: 64);

            
            return PolygonPolygonIntersectionArea(circleApprox, polygon);
        }

        
        private static Polygon ApproximateCircleAsPolygon(Circle circle, int sides = 64)
        {
            var points = new List<Point>(sides);
            Point center = circle.Center();
            float r = circle.Radius();
            float angleStep = 2f * MathF.PI / sides;

            for (int i = 0; i < sides; i++)
            {
                float angle = i * angleStep;
                float x = center.Horisontal() + r * MathF.Cos(angle);
                float y = center.Vertical() + r * MathF.Sin(angle);
                points.Add(new Point(x, y));
            }

            return new Polygon(points);
        }

        static bool PointInPolygon(Point p, Polygon poly)
        {
            bool inside = false;
            var pts = poly.Points();
            int j = pts.Length - 1;

            for (int i = 0; i < pts.Length; i++)
            {
                bool inter =
                    (pts[i].Vertical() > p.Vertical()) != (pts[j].Vertical() > p.Vertical()) &&
                    (p.Horisontal() <
                     (pts[j].Horisontal() - pts[i].Horisontal()) *
                     (p.Vertical() - pts[i].Vertical()) /
                     (pts[j].Vertical() - pts[i].Vertical()) +
                     pts[i].Horisontal());
                if (inter) inside = !inside;
                j = i;
            }
            return inside;
        }

        static float PolygonArea(List<Point> pts)
        {
            float sum = 0;
            int j = pts.Count - 1;
            for (int i = 0; i < pts.Count; i++)
            {
                sum += (pts[j].Horisontal() + pts[i].Horisontal()) *
                       (pts[j].Vertical() - pts[i].Vertical());
                j = i;
            }
            return Math.Abs(sum / 2f);
        }
    }
}
