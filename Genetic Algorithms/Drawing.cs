using System.Drawing;
using CircleLib;
using PolygonLib;
using IndividualLib;
using PointLib;

namespace DrawingLib
{
    public static class Drawings
    {
        
        public static void Draw(Individual individual, PaintEventArgs e)
        {
            foreach (var shape in individual.Shapes)
            {
                DrawShape((dynamic)shape, e.Graphics);
            }
        }

        private static void DrawShape(Circle circle, Graphics g)
        {
            float x = circle.Center().Horisontal() - circle.Radius();
            float y = circle.Center().Vertical() - circle.Radius();
            float d = 2 * circle.Radius();

            g.DrawEllipse(Pens.Black, x, y, d, d);
        }

        private static void DrawShape(Polygon polygon, Graphics g)
        {
            var points = polygon.Points()
                .Select(p => new PointF(p.Horisontal(), p.Vertical()))
                .ToArray();

            g.DrawPolygon(Pens.Black, points);
        }
    }
}