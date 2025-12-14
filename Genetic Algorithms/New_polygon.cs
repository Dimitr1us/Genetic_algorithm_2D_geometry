using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IndividualLib;
using SPoint =  PointLib.Point;
namespace Genetic_Algorithms
{
    public partial class New_polygon : Form
    {
        public event Action<List<SPoint>> newPoints;
        string X, Y;
        int number;
        List<SPoint> points;
        public New_polygon()
        {
            points = new List<SPoint>();
            number = 0;
            InitializeComponent();
        }

        private void New_polygon_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            X = textBox1.Text;
            button1.Visible = isVisible();
            label3.Visible = !isVisible();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Y = textBox2.Text;
            button1.Visible = isVisible();
            label3.Visible = !isVisible();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            number++;
            points.Add(new SPoint(float.Parse(X), float.Parse(Y)));
            listBox1.Items.Add($"({X}; {Y})");
            textBox1.Text = "";
            textBox2.Text = "";
            if (number > 2) button2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            newPoints.Invoke(points);
            this.Close();
        }

        private bool isVisible()
        {
            bool val =  float.TryParse(X, out float value) && float.TryParse(Y, out float value1);
            if (val) val = float.Parse(X) >= 0 && float.Parse(Y) >= 0;
            return val;
        }
    }
}
