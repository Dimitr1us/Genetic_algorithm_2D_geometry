using System.Xml.Serialization;
using CrossShapesLib;
using System.Drawing;
using PopulationLib;
using IndividualLib;
using DrawingLib;
using SPoint = PointLib.Point;
using SPolygon = PolygonLib.Polygon;
using CircleLib;
using Microsoft.VisualBasic.Logging;
using PolygonLib;
namespace Genetic_Algorithms
{

    public partial class Form1 : Form
    {
        private readonly Random rnd = new();
        int width, height;
        Population population;

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public Form1(int width=400,int height=400,int individuals=80)
        {
            population = new Population(individuals,width,height);
            this.width = width; this.height = height;
            InitializeComponent();
            this.DoubleBuffered = true;

            var circle = new Circle(new PointLib.Point(200, 200), 40);
            var square = new Polygon(new List<PointLib.Point>
        {
            new(0,0), new(80,0), new(80,80), new(0,80)
        });
            var triangle = new Polygon(new List<PointLib.Point>
        {
            new(0,0), new(100,0), new(50,86)
        });

            population.Add(circle);
            population.Add(square);
            population.Add(triangle);

            population.InitializeRandom();
            panel1.Paint += panel1_Paint;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            population.NextGeneration(mutationRate: 0.3);

            label1.Text = $"Generation: {population.Generation}";
            label2.Text = $"Best: {population.Best.Fitness():F2}";
            label3.Text = $"Avg: {population.AverageFitness():F2}";

            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            Drawings.Draw(population.Best, e);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            New_circle new_Circle = new New_circle();
            new_Circle.NewRadius += (radius) =>
            {
                Circle circle = new Circle(new SPoint(rnd.Next(width), rnd.Next(height)), radius);
                population.Add(circle);
            };
            new_Circle.ShowDialog();
            panel1.Invalidate();
        }
    }
}
