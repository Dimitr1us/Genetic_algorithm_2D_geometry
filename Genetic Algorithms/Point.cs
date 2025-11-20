using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointLib
{
    public class Point
    {
        private float X, Y;
        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point(Point point)
        {
            X = point.Horisontal();
            Y = point.Vertical();
        }

        public float Horisontal()
        {
            return X;
        }

        public float Vertical()
        {
            return Y;
        }

        public void Move(float x, float y)
        {
            X = X + x;
            Y = Y + y;
        }

        public float Distance(Point point)
        {
            return (float)Math.Sqrt(Math.Pow(this.Horisontal() - point.Horisontal(), 2) + Math.Pow(this.Vertical() - point.Vertical(), 2));
        }
    }
}
