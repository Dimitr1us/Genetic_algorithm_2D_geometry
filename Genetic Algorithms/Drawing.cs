using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleLib;
using IndividualLib;
using PolygonLib;
using SPoint = PointLib.Point;
using SPolygon = PolygonLib.Polygon;
namespace DrawingLib
{
    public static class Drawings
    {
        public static void Draw(Individual individual, PaintEventArgs e)
        {
            for (int i = 0; i < individual.Count(); i++)
            {
                DrawShape((dynamic)individual[i], e);
            }
        }

        public static void DrawShape(Circle circle, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawEllipse(Pens.Black, circle.Center().Horisontal() - circle.Radius(), circle.Center().Vertical() - circle.Radius(), 2 * circle.Radius(), 2 * circle.Radius());
        }

        public static void DrawShape(SPolygon polygon, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SPoint[] spoints = polygon.Points();
            System.Drawing.Point[] points = new System.Drawing.Point[spoints.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new System.Drawing.Point(Convert.ToInt32(spoints[i].Horisontal()), Convert.ToInt32(spoints[i].Vertical()));
            }
            g.DrawPolygon(Pens.Black, points);
        }
    }
}
