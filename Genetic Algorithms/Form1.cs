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
        Population population = new(80, 400, 400);

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //Drawings.Draw(population[0], e);
            //e.Graphics.DrawPolygon(Pens.Black, new System.Drawing.Point[] { new System.Drawing.Point(0, 0), new System.Drawing.Point(50, 0), new System.Drawing.Point(50, 50), new System.Drawing.Point(0, 50) });
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public Form1()
        {
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
    }
}
