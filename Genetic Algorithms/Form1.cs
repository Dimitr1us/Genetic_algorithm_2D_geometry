using System.Xml.Serialization;
using CrossShapesLib;
using System.Drawing;
using PopulationLib;
using IndividualLib;
using iShapeLib;
using SPoint = PointLib.Point;
using SPolygon = PolygonLib.Polygon;
using CircleLib;
using Microsoft.VisualBasic.Logging;
namespace Genetic_Algorithms
{

    public partial class Form1 : Form
    {

        Population population = new Population(120,3,400,400);
        public Form1()
        {
            InitializeComponent();
            label1.Text = "0";
            this.Width = 400;
            this.Height = 400;

            this.DoubleBuffered = true;
            Circle circle = new(new(100, 100), 50);
            population.Add(circle);
            SPolygon square = new SPolygon(new List<SPoint>
                {
                    new SPoint(0, 0),
                    new SPoint(50, 0),
                    new SPoint(50, 50),
                    new SPoint(0, 50)
                });

            
            SPolygon triangle = new SPolygon(new List<SPoint>
                {
                    new SPoint(400, 200),
                    new SPoint(500, 300),
                    new SPoint(300, 300)
                });


            population.Add(triangle);
            population.Add(square);
            this.Paint += new PaintEventHandler(this.OnPaint);

            population.Mix();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Drawings.Draw(population[0], e);
            //e.Graphics.DrawPolygon(Pens.Black, new System.Drawing.Point[] { new System.Drawing.Point(0, 0), new System.Drawing.Point(50, 0), new System.Drawing.Point(50, 50), new System.Drawing.Point(0, 50) });
        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            float x = 0.05f;
            population.Round(x);
            label1.Text = population.GetRound().ToString();
            //label2.Text = population.Max().ToString();
            label2.Text = population[0].Fitness().ToString();
            float sum = 0;
            for (int i = 0; i < 120; i++) { sum = sum + population[i].Fitness(); }
            label3.Text = (sum / 120).ToString();
            this.Invalidate();
        }
    }

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
            g.DrawEllipse(Pens.Black, circle.Center().Horisontal() - circle.Radius(), circle.Center().Vertical() - circle.Radius(),2*circle.Radius(),2*circle.Radius());
        }

        public static void DrawShape(SPolygon polygon, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SPoint[] spoints = polygon.Points();
            System.Drawing.Point[] points = new System.Drawing.Point[spoints.Length];
            for (int i = 0;i < points.Length; i++)
            {
                points[i] = new System.Drawing.Point(Convert.ToInt32(spoints[i].Horisontal()), Convert.ToInt32(spoints[i].Vertical()));
            }


            g.DrawPolygon(Pens.Black,points);
        }
    }
}
