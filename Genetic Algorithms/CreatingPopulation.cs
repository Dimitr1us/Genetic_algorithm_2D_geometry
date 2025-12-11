using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Genetic_Algorithms
{
    public partial class CreatingPopulation : Form
    {
        string width, height, individuals;

        public CreatingPopulation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(int.Parse(width), int.Parse(height), int.Parse(individuals));
            form1.ShowDialog();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            width = textBox1.Text;
            label4.Visible = isVisible() ? false : true;
            button1.Visible = isVisible() ? true : false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            height = textBox2.Text;
            label4.Visible = isVisible() ? false : true;
            button1.Visible = isVisible() ? true : false;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            individuals = textBox3.Text;
            label4.Visible = isVisible() ? false : true;
            button1.Visible = isVisible() ? true : false;
        }

        private bool isVisible()
        {
            return int.TryParse(width, out int value) && int.TryParse(height, out int value1) && int.TryParse(individuals, out int value2);
        }
    }
}
