using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CircleLib;
namespace Genetic_Algorithms
{
    public partial class New_circle : Form
    {
        public event Action<float> NewRadius;
        string entered_Value;
        public New_circle()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            entered_Value = textBox1.Text;
            if (!float.TryParse(entered_Value, out float value))
            {
                label2.Visible = true; 
            }
            else
            {
                label2.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (float.TryParse(entered_Value, out float value))
            {
                NewRadius.Invoke(float.Parse(entered_Value));
                this.Close();
            }
            else
            {
                label1.Visible = false;
            }
        }
    }
}
