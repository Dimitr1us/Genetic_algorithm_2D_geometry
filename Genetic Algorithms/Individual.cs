using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossShapesLib;
using iShapeLib;
using PointLib;
namespace IndividualLib
{
    public class Individual
    {
        int id;
        List<iShape> shapes;
        int width;
        int height;
        private static Random random = new Random();

        public iShape this[int index]
        {
            get => shapes[index];
            set => shapes[index] = value;
        }


        public Individual(int x,int y,int id) { 
            shapes = new List<iShape>();
            width = x;
            height = y;
            this.id = id;
        }

        public int Id()
        {
            return id;
        }


        public void Add(iShape shape)
        {
            shapes.Add(shape);
        }

        public void Mix()
        {
            foreach (iShape shape in shapes)
            {
                shape.Put(random.Next(0,width),random.Next(0,height));
                shape.Rotate(random.Next(0,360));
            }
        }

        public void Intersection(Individual individ)
        {
            //iShape shape = shapes[i];
            //shapes[i].Put(individ[i].Center().Horisontal(),individ[i].Center().Vertical());
            //individ[i].Put(shape.Center().Horisontal(), shape.Center().Vertical());
            for (int i = 0; i < individ.Count() / 2; i++) { this[i] = individ[i].Clone(); }
            for (int i = individ.Count()/2; i< individ.Count(); i++) { individ[i] = this[i].Clone(); }
        }

        public void Mutation()
        {
            int i = random.Next(0, shapes.Count());
            shapes[i].Put(random.Next((int)shapes[i].Center().Horisontal()-10, (int)shapes[i].Center().Horisontal() + 10),random.Next((int)shapes[i].Center().Vertical() - 10, (int)shapes[i].Center().Vertical() + 10));
        }

        public int Count()
        {
            return shapes.Count();
        }

        public float Fitness(float weightIntersect = 1, float weightNonIntersect = 1)
        {
            float sum = 0;

            for (int i = 0; i < shapes.Count; i++)
            {
                if (shapes[i].CrossLines(width, height)) sum += -10;
                for (int j = i + 1; j < shapes.Count; j++)
                {
                    bool cross = CrossShape.Cross(shapes[i], shapes[j]);

                    if (cross)
                        sum += weightIntersect * -1;
                    else
                        sum += weightNonIntersect * 1;
                }
            }

            return sum;
        }
    }
}
