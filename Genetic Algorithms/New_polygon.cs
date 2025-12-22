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
using SPoint = PointLib.Point;

namespace Genetic_Algorithms
{
    /// <summary>
    /// Диалоговое окно для ввода координат вершин нового многоугольника.
    /// </summary>
    public partial class New_polygon : Form
    {
        /// <summary>
        /// Событие, вызываемое при подтверждении ввода точек многоугольника.
        /// Передаёт список введённых точек.
        /// </summary>
        public event Action<List<SPoint>> newPoints;

        private string X;
        private string Y;
        private int number;
        private List<SPoint> points;

        /// <summary>
        /// Инициализирует новое диалоговое окно для создания многоугольника.
        /// </summary>
        public New_polygon()
        {
            points = new List<SPoint>();
            number = 0;
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события загрузки формы.
        /// </summary>
        private void New_polygon_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Обработчик клика по метке (оставлен для совместимости).
        /// </summary>
        private void label1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Обработчик изменения текста в поле ввода координаты X.
        /// Обновляет видимость кнопки и метки ошибки.
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            X = textBox1.Text;
            button1.Visible = isVisible();
            label3.Visible = !isVisible();
        }

        /// <summary>
        /// Обработчик изменения текста в поле ввода координаты Y.
        /// Обновляет видимость кнопки и метки ошибки.
        /// </summary>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Y = textBox2.Text;
            button1.Visible = isVisible();
            label3.Visible = !isVisible();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Добавить точку".
        /// Добавляет точку в список, отображает её в listBox и очищает поля ввода.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            number++;
            points.Add(new SPoint(float.Parse(X), float.Parse(Y)));
            listBox1.Items.Add($"({X}; {Y})");
            textBox1.Text = "";
            textBox2.Text = "";
            if (number > 2) button2.Visible = true;
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Создать".
        /// Вызывает событие <see cref="newPoints"/> с введёнными точками и закрывает форму.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            newPoints.Invoke(points);
            this.Close();
        }

        /// <summary>
        /// Проверяет корректность введённых координат.
        /// </summary>
        /// <returns>
        /// <see langword="true"/>, если обе координаты — неотрицательные числа; иначе <see langword="false"/>.
        /// </returns>
        private bool isVisible()
        {
            bool val = float.TryParse(X, out float value) && float.TryParse(Y, out float value1);
            if (val) val = float.Parse(X) >= 0 && float.Parse(Y) >= 0;
            return val;
        }
    }
}