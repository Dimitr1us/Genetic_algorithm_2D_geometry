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
    /// <summary>
    /// Начальная форма для ввода параметров создания популяции: ширины, высоты поля и количества особей.
    /// </summary>
    public partial class CreatingPopulation : Form
    {
        string width, height, individuals;

        /// <summary>
        /// Инициализирует новую форму для ввода параметров популяции.
        /// </summary>
        public CreatingPopulation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Создать".
        /// Создаёт главную форму с введёнными параметрами и показывает её, скрывая текущую форму.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(int.Parse(width), int.Parse(height), int.Parse(individuals));
            form1.Show();
            this.Hide();
        }

        /// <summary>
        /// Обработчик изменения текста в поле ширины.
        /// Обновляет состояние кнопки и метки ошибки.
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            width = textBox1.Text;
            label4.Visible = isVisible() ? false : true;
            button1.Visible = isVisible() ? true : false;
        }

        /// <summary>
        /// Обработчик изменения текста в поле высоты.
        /// Обновляет состояние кнопки и метки ошибки.
        /// </summary>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            height = textBox2.Text;
            label4.Visible = isVisible() ? false : true;
            button1.Visible = isVisible() ? true : false;
        }

        /// <summary>
        /// Обработчик изменения текста в поле количества особей.
        /// Обновляет состояние кнопки и метки ошибки.
        /// </summary>
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            individuals = textBox3.Text;
            label4.Visible = isVisible() ? false : true;
            button1.Visible = isVisible() ? true : false;
        }

        /// <summary>
        /// Проверяет корректность введённых значений.
        /// </summary>
        /// <returns>
        /// <see langword="true"/>, если все поля содержат положительные целые числа; иначе <see langword="false"/>.
        /// </returns>
        private bool isVisible()
        {
            var val = int.TryParse(width, out int value) &&
                      int.TryParse(height, out int value1) &&
                      int.TryParse(individuals, out int value2);
            if (val) val = int.Parse(width) > 0 && int.Parse(height) > 0 && int.Parse(individuals) > 0;
            return val;
        }
    }
}