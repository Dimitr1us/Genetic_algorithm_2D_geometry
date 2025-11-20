using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = PointLib.Point;
namespace iShapeLib
{
    public interface iShape
    {
        public void Move(float x, float y);
        public Point Center();

        public void Rotate(float degrees);

        public void Put(float x, float y);
    }
}
