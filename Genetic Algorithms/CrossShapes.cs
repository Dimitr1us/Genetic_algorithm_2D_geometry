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
    /// <summary>
    /// Статический класс для вычисления площади пересечения двух фигур.
    /// Поддерживает пересечения кругов, многоугольников и их комбинаций.
    /// </summary>
    public static class CrossShape
    {
        /// <summary>
        /// Вычисляет площадь пересечения двух фигур любого типа (<see cref="iShape"/>).
        /// Использует динамическую диспетчеризацию для вызова специализированного метода.
        /// </summary>
        /// <param name="s1">Первая фигура.</param>
        /// <param name="s2">Вторая фигура.</param>
        /// <returns>Площадь пересечения (в условных единицах). Если фигур нет пересечения — 0.</returns>
        /// <remarks>
        /// Поддерживаемые типы: <see cref="Circle"/>, <see cref="Polygon"/>.
        /// При неизвестных типах будет выброшено исключение.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Если типы фигур не поддерживаются.</exception>
        public static float Cross(iShape s1, iShape s2) => Cross((dynamic)s1, (dynamic)s2);

        /// <summary>
        /// Вычисляет площадь пересечения двух кругов.
        /// </summary>
        /// <param name="c1">Первый круг.</param>
        /// <param name="c2">Второй круг.</param>
        /// <returns>Площадь пересечения в квадратных единицах.</returns>
        public static float Cross(Circle c1, Circle c2)
            => CircleIntersectionArea(c1, c2);

        /// <summary>
        /// Вычисляет площадь пересечения двух многоугольников.
        /// </summary>
        /// <param name="p1">Первый многоугольник.</param>
        /// <param name="p2">Второй многоугольник.</param>
        /// <returns>Площадь пересечения.</returns>
        public static float Cross(Polygon p1, Polygon p2)
            => PolygonPolygonIntersectionArea(p1, p2);

        /// <summary>
        /// Вычисляет площадь пересечения круга и многоугольника.
        /// </summary>
        /// <param name="c">Круг.</param>
        /// <param name="p">Многоугольник.</param>
        /// <returns>Площадь пересечения.</returns>
        public static float Cross(Circle c, Polygon p)
            => CirclePolygonIntersectionArea(c, p);

        /// <summary>
        /// Вычисляет площадь пересечения многоугольника и круга (симметрично).
        /// </summary>
        /// <param name="p">Многоугольник.</param>
        /// <param name="c">Круг.</param>
        /// <returns>Площадь пересечения.</returns>
        public static float Cross(Polygon p, Circle c)
            => CirclePolygonIntersectionArea(c, p);

        /// <summary>
        /// Вычисляет значение линейной функции в точке x.
        /// </summary>
        /// <param name="a">Коэффициент наклона.</param>
        /// <param name="b">Свободный член.</param>
        /// <param name="x">Абсцисса точки.</param>
        /// <returns>Значение y = a*x + b.</returns>
        public static float Line(float a, float b, float x) => a * x + b;

        /// <summary>
        /// Вычисляет параметры прямой (a, b) по двум точкам.
        /// </summary>
        /// <param name="p1">Первая точка.</param>
        /// <param name="p2">Вторая точка.</param>
        /// <returns>Кортеж (a — наклон, b — свободный член).</returns>
        /// <exception cref="DivideByZeroException">Если точки имеют одинаковую x-координату.</exception>
        public static (float a, float b) Parametres(Point p1, Point p2)
        {
            float a = (p1.Vertical() - p2.Vertical()) / (p1.Horisontal() - p2.Horisontal());
            float b = p1.Vertical() - a * p1.Horisontal();
            return (a, b);
        }

        /// <summary>
        /// Находит точку пересечения двух прямых, заданных уравнениями y = a*x + b.
        /// </summary>
        /// <param name="L1">Параметры первой прямой (a1, b1).</param>
        /// <param name="L2">Параметры второй прямой (a2, b2).</param>
        /// <returns>Точка пересечения.</returns>
        /// <remarks>Если прямые параллельны, возвращается некорректное значение.</remarks>
        public static Point Intersection((float a, float b) L1, (float a, float b) L2)
        {
            float x = (L2.b - L1.b) / (L1.a - L2.a);
            return new Point(x, L1.a * x + L1.b);
        }

        /// <summary>
        /// Вычисляет площадь пересечения двух кругов по формуле аналитической геометрии.
        /// </summary>
        /// <param name="c1">Первый круг.</param>
        /// <param name="c2">Второй круг.</param>
        /// <returns>Площадь пересечения.</returns>
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

        /// <summary>
        /// Вычисляет площадь пересечения двух многоугольников с помощью алгоритма Sutherland-Hodgman.
        /// </summary>
        /// <param name="a">Первый многоугольник.</param>
        /// <param name="b">Второй многоугольник.</param>
        /// <returns>Площадь пересечения.</returns>
        private static float PolygonPolygonIntersectionArea(Polygon a, Polygon b)
        {
            var inter = SutherlandHodgman(a.Points().ToList(), b.Points().ToList());
            if (inter.Count < 3) return 0;
            return PolygonArea(inter);
        }

        /// <summary>
        /// Выполняет отсечение многоугольника по алгоритму Sutherland-Hodgman.
        /// </summary>
        /// <param name="subject">Отсекаемый многоугольник.</param>
        /// <param name="clip">Отсекающий многоугольник.</param>
        /// <returns>Список точек результирующего многоугольника пересечения.</returns>
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
                    {
                        output.Add(LineIntersection(prev, curr, A, B));
                    }
                    prev = curr;
                }
            }
            return output;
        }

        /// <summary>
        /// Проверяет, находится ли точка внутри полуплоскости, заданной ребром (A, B).
        /// </summary>
        private static bool Inside(Point p, Point a, Point b)
            => (b.Horisontal() - a.Horisontal()) * (p.Vertical() - a.Vertical())
             - (b.Vertical() - a.Vertical()) * (p.Horisontal() - a.Horisontal())
               >= 0;

        /// <summary>
        /// Вычисляет точку пересечения двух отрезков.
        /// </summary>
        private static Point LineIntersection(Point s, Point e, Point a, Point b)
        {
            float A1 = e.Vertical() - s.Vertical();
            float B1 = s.Horisontal() - e.Horisontal();
            float C1 = A1 * s.Horisontal() + B1 * s.Vertical();

            float A2 = b.Vertical() - a.Vertical();
            float B2 = a.Horisontal() - b.Horisontal();
            float C2 = A2 * a.Horisontal() + B2 * a.Vertical();

            float det = A1 * B2 - A2 * B1;
            if (Math.Abs(det) < 1e-6) return s; // Параллельны

            float x = (B2 * C1 - B1 * C2) / det;
            float y = (A1 * C2 - A2 * C1) / det;
            return new Point(x, y);
        }

        /// <summary>
        /// Вычисляет площадь пересечения круга и многоугольника.
        /// </summary>
        /// <param name="circle">Круг.</param>
        /// <param name="polygon">Многоугольник.</param>
        /// <returns>Площадь пересечения.</returns>
        /// <remarks>Круг аппроксимируется многоугольником с 64 сторонами.</remarks>
        static float CirclePolygonIntersectionArea(Circle circle, Polygon polygon)
        {
            Polygon circleApprox = ApproximateCircleAsPolygon(circle, sides: 64);
            return PolygonPolygonIntersectionArea(circleApprox, polygon);
        }

        /// <summary>
        /// Аппроксимирует круг многоугольником с заданным количеством сторон.
        /// </summary>
        /// <param name="circle">Круг.</param>
        /// <param name="sides">Количество сторон (по умолчанию 64).</param>
        /// <returns>Многоугольник, аппроксимирующий круг.</returns>
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

        /// <summary>
        /// Проверяет, находится ли точка внутри многоугольника (алгоритм Ray Casting).
        /// </summary>
        private static bool PointInPolygon(Point p, Polygon poly)
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

        /// <summary>
        /// Вычисляет площадь многоугольника по формуле Гаусса (shoelace).
        /// </summary>
        /// <param name="pts">Список точек многоугольника (в порядке обхода).</param>
        /// <returns>Площадь многоугольника.</returns>
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