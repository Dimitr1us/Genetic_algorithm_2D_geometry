using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPolygonalLib;
using iShapeLib;
using ShapesLib;
using SideLib;
using Point = PointLib.Point;
namespace PolygonLib
{
    public class Polygon : iPolygonal, iShape
    {
        private List<Side> sides;
        private Point center;
        public Polygon(List<Point> points)
        {
            sides = new List<Side>();
            float x = 0;
            float y = 0;
            for (int i = 0; i < points.Count - 1; i++)
            {
                sides.Add(new Side(new Point(points[i].Horisontal(), points[i].Vertical()), new Point(points[i + 1].Horisontal(), points[i + 1].Vertical())));
                x = x + points[i].Horisontal();
                y = y + points[i].Vertical();
            }
            sides.Add(new Side(new Point(points[points.Count - 1].Horisontal(), points[points.Count - 1].Vertical()), new Point(points[0].Horisontal(), points[0].Vertical())));
            x += points[points.Count - 1].Horisontal();
            y += points[points.Count - 1].Vertical();
            center = new Point(x / points.Count, y / points.Count);
        }
        public Point Center()
        {
            return this.center;
        }

        public Polygon Clone()
        {
            Point[] pts = this.Points();

            List<Point> newPoints = new List<Point>();
            foreach (Point p in pts)
            {
                newPoints.Add(new Point(p.Horisontal(), p.Vertical()));
            }

            return new Polygon(newPoints);
        }

        public void Move(float x, float y)
        {
            this.center.Move(x, y);
            foreach (Side side in sides)
            {
                side.Move(x, y);
            }
        }

        public Point[] Points()
        {
            Point[] points = new Point[sides.Count];
            for (int i = 0; i < sides.Count; i++)
            {
                points[i] = sides[i].FirstPoint();
            }
            return points;
        }

        public List<Side> Sides()
        {
            return sides;
        }

        public void Put(float x, float y)
        {
            this.Move(x - this.Center().Horisontal(), y - this.Center().Vertical());
        }

        public Side NearistSide(Point point)
        {
            float max = float.MaxValue;
            Side neededSide = null;
            foreach (Side side in sides)
            {
                if (side.Distance(point) < max)
                {
                    max = side.Distance(point);
                    neededSide = side;
                }
            }
            return neededSide;
        }

        public float DistanceFromCenterToSide(Point point, Side side)
        {
            (float center_a, float center_b) = CrossShape.Parametres(this.center, point);
            (float side_a, float side_b) = CrossShape.Parametres(side.FirstPoint(), side.SecondPoint());
            Point pointInSide = CrossShape.Intersection((center_a, center_b), (side_a, side_b));
            return pointInSide.Distance(this.center);
        }

        public void Rotate(float degrees)
        {
            float rad = degrees * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);
            Point c = this.Center();

            foreach (Side side in sides)
            {
                RotatePoint(side.FirstPoint(), c, cos, sin);
                RotatePoint(side.SecondPoint(), c, cos, sin);
            }
        }

        private void RotatePoint(Point p, Point center, float cos, float sin)
        {
            float x = p.Horisontal() - center.Horisontal();
            float y = p.Vertical() - center.Vertical();
            float newX = x * cos - y * sin + center.Horisontal();
            float newY = x * sin + y * cos + center.Vertical();

            p.Move(newX - p.Horisontal(), newY - p.Vertical());
        }


    }
}
