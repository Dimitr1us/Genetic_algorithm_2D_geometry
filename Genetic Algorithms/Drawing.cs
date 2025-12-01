// DrawingLib/Drawings.cs — РАБОЧАЯ ВЕРСИЯ ПОД НОВЫЙ Individual
using System.Drawing;
using CircleLib;
using PolygonLib;
using IndividualLib;
using PointLib;

namespace DrawingLib
{
    public static class Drawings
    {
        // Главный метод — теперь принимает Individual из нового кода
        public static void Draw(Individual individual, PaintEventArgs e)
        {
            foreach (var shape in individual.Shapes)
            {
                // dynamic позволяет рисовать и Circle, и Polygon без if-ов
                DrawShape((dynamic)shape, e.Graphics);
            }
        }

        // Рисуем круг
        private static void DrawShape(Circle circle, Graphics g)
        {
            float x = circle.Center().Horisontal() - circle.Radius();
            float y = circle.Center().Vertical() - circle.Radius();
            float d = 2 * circle.Radius();

            g.DrawEllipse(Pens.Black, x, y, d, d);
            // Можно добавить заливку:
            // g.FillEllipse(new SolidBrush(Color.FromArgb(100, 255, 100, 100)), x, y, d, d);
        }

        // Рисуем полигон
        private static void DrawShape(Polygon polygon, Graphics g)
        {
            var points = polygon.Points()
                .Select(p => new PointF(p.Horisontal(), p.Vertical()))
                .ToArray();

            g.DrawPolygon(Pens.Black, points);
            // Заливка (полупрозрачная):
            // g.FillPolygon(new SolidBrush(Color.FromArgb(120, 100, 150, 255)), points);
        }
    }
}