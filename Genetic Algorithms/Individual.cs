// Individual.cs — ЧИСТАЯ РАБОЧАЯ ВЕРСИЯ
using System;
using System.Collections.Generic;
using CrossShapesLib;
using iShapeLib;
using PointLib;

namespace IndividualLib
{
    public class Individual
    {
        private static readonly Random rnd = new Random();
        private readonly List<iShape> shapes = new();
        public int Width { get; }
        public int Height { get; }
        public IReadOnlyList<iShape> Shapes => shapes;

        public Individual(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Add(iShape shape) => shapes.Add(shape.Clone());

        public void Randomize()
        {
            foreach (var s in shapes)
            {
                s.Put(rnd.Next(50, Width - 50), rnd.Next(50, Height - 50));
                s.Rotate(rnd.Next(0, 360));
            }
        }

        public Individual Crossover(Individual other)
        {
            var child = new Individual(Width, Height);

            int max = Math.Max(shapes.Count, other.shapes.Count);
            for (int i = 0; i < max; i++)
            {
                if (i >= shapes.Count) child.shapes.Add(other.shapes[i].Clone());
                else if (i >= other.shapes.Count) child.shapes.Add(shapes[i].Clone());
                else child.shapes.Add(rnd.NextDouble() < 0.5 ? shapes[i].Clone() : other.shapes[i].Clone());
            }
            return child;
        }

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
                    if (rnd.NextDouble() < 0.5) s.Rotate(rnd.Next(0, 360));
                }
            }
        }

        public float Fitness()
        {
            if (shapes.Count < 2) return 0f;
            float penalty = 0f;
            for (int i = 0; i < shapes.Count; i++)
                for (int j = i + 1; j < shapes.Count; j++)
                    penalty += CrossShape.Cross(shapes[i], shapes[j]);
            return -penalty;
        }

        public Individual Clone()
        {
            var c = new Individual(Width, Height);
            foreach (var s in shapes) c.shapes.Add(s.Clone());
            return c;
        }
    }
}