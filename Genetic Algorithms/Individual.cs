using System;
using System.Collections.Generic;
using CrossShapesLib;
using iShapeLib;
using PointLib;

namespace IndividualLib
{
    /// <summary>
    /// Представляет одну особь (индивидуум) в генетическом алгоритме.
    /// Хранит набор фигур (<see cref="iShape"/>) и управляет их позицией, поворотом, кроссовером и мутацией.
    /// </summary>
    public class Individual
    {
        private static readonly Random rnd = new Random();
        private readonly List<iShape> shapes = new();

        /// <summary>
        /// Ширина поля, на котором размещаются фигуры.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Высота поля, на котором размещаются фигуры.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Неизменяемый список всех фигур, принадлежащих данной особи.
        /// </summary>
        public IReadOnlyList<iShape> Shapes => shapes;

        /// <summary>
        /// Создаёт новую пустую особь с заданными размерами поля.
        /// </summary>
        /// <param name="width">Ширина поля (в пикселях или условных единицах).</param>
        /// <param name="height">Высота поля (в пикселях или условных единицах).</param>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="width"/> или <paramref name="height"/> меньше 1.</exception>
        public Individual(int width, int height)
        {
            if (width < 1) throw new ArgumentOutOfRangeException(nameof(width), "Ширина должна быть положительной.");
            if (height < 1) throw new ArgumentOutOfRangeException(nameof(height), "Высота должна быть положительной.");

            Width = width;
            Height = height;
        }

        /// <summary>
        /// Добавляет копию фигуры в список особей.
        /// </summary>
        /// <param name="shape">Фигура, которую нужно добавить (будет создана её копия).</param>
        public void Add(iShape shape) => shapes.Add(shape.Clone());

        /// <summary>
        /// Случайно размещает все фигуры в пределах поля и поворачивает их на случайный угол.
        /// </summary>
        /// <remarks>
        /// Фигуры размещаются в центре поля с отступом 50 единиц от краёв.
        /// </remarks>
        public void Randomize()
        {
            foreach (var s in shapes)
            {
                s.Put(rnd.Next(50, Width - 50), rnd.Next(50, Height - 50));
                s.Rotate(rnd.Next(0, 360));
            }
        }

        /// <summary>
        /// Выполняет кроссовер (скрещивание) с другой особью.
        /// </summary>
        /// <param name="other">Другая особь для скрещивания.</param>
        /// <returns>Новая особь-потомок, полученная путём одноточечного кроссовера по фигурам.</returns>
        /// <remarks>
        /// Для каждой позиции выбирается фигура случайно из одной из двух родительских особей.
        /// Если у одной особи меньше фигур, остальные берутся из другой.
        /// </remarks>
        public Individual Crossover(Individual other)
        {
            var child = new Individual(Width, Height);
            int max = Math.Max(shapes.Count, other.shapes.Count);

            for (int i = 0; i < max; i++)
            {
                if (i >= shapes.Count)
                    child.shapes.Add(other.shapes[i].Clone());
                else if (i >= other.shapes.Count)
                    child.shapes.Add(shapes[i].Clone());
                else
                    child.shapes.Add(rnd.NextDouble() < 0.5 ? shapes[i].Clone() : other.shapes[i].Clone());
            }

            return child;
        }

        /// <summary>
        /// Выполняет мутацию фигуры с заданной вероятностью.
        /// </summary>
        /// <param name="probability">Вероятность мутации для каждой фигуры (от 0.0 до 1.0). По умолчанию 0.3.</param>
        /// <remarks>
        /// Мутация изменяет положение фигуры (±70 единиц) и с вероятностью 50% — поворот на случайный угол.
        /// Координаты ограничены отступом 30 единиц от краёв поля.
        /// </remarks>
        public void Mutate(double probability = 0.3)
        {
            foreach (var s in shapes)
            {
                if (rnd.NextDouble() < probability)
                {
                    int x = (int)s.Center().Horisontal() + rnd.Next(-70, 71);
                    int y = (int)s.Center().Vertical() + rnd.Next(-70, 71);

                    x = Math.Clamp(x, 30, Width - 30);
                    y = Math.Clamp(y, 30, Height - 30);

                    s.Put(x, y);

                    if (rnd.NextDouble() < 0.5)
                        s.Rotate(rnd.Next(0, 360));
                }
            }
        }

        /// <summary>
        /// Вычисляет приспособленность (fitness) особи.
        /// </summary>
        /// <returns>
        /// Отрицательное значение суммы штрафов за пересечения всех пар фигур.
        /// Чем меньше пересечений — тем выше приспособленность (меньше штраф).
        /// </returns>
        /// <remarks>
        /// Если особь содержит менее 2 фигур, возвращает 0.
        /// </remarks>
        public float Fitness()
        {
            if (shapes.Count < 2) return 0f;

            float penalty = 0f;
            for (int i = 0; i < shapes.Count; i++)
            {
                for (int j = i + 1; j < shapes.Count; j++)
                {
                    penalty += CrossShape.Cross(shapes[i], shapes[j]);
                }
            }

            return -penalty;
        }

        /// <summary>
        /// Создаёт полную глубокую копию данной особи.
        /// </summary>
        /// <returns>Новая особь с идентичным набором фигур (глубокое копирование).</returns>
        public Individual Clone()
        {
            var c = new Individual(Width, Height);
            foreach (var s in shapes)
            {
                c.shapes.Add(s.Clone());
            }
            return c;
        }
    }
}