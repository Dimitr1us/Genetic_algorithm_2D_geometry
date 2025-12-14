using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
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
            label2.Visible = float.TryParse(entered_Value, out float value)  ? false : true;
            if (!label2.Visible)
                if(float.Parse(entered_Value) <= 0) label2.Visible = true;
            button1.Visible = !label2.Visible;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewRadius.Invoke(float.Parse(entered_Value));
            this.Close();
        }
    }
}
