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
    /// <summary>
    /// Диалоговое окно для ввода радиуса нового круга.
    /// </summary>
    public partial class New_circle : Form
    {
        /// <summary>
        /// Событие, вызываемое при подтверждении ввода радиуса.
        /// Передаёт введённое значение радиуса.
        /// </summary>
        public event Action<float> NewRadius;

        private string entered_Value;

        /// <summary>
        /// Инициализирует новое диалоговое окно для ввода радиуса круга.
        /// </summary>
        public New_circle()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик изменения текста в поле ввода радиуса.
        /// Обновляет видимость метки ошибки и кнопки подтверждения.
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            entered_Value = textBox1.Text;
            label2.Visible = float.TryParse(entered_Value, out float value) ? false : true;
            if (!label2.Visible)
                if (float.Parse(entered_Value) <= 0) label2.Visible = true;
            button1.Visible = !label2.Visible;
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Добавить".
        /// Вызывает событие <see cref="NewRadius"/> с введённым значением радиуса и закрывает форму.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            NewRadius.Invoke(float.Parse(entered_Value));
            this.Close();
        }
    }
}