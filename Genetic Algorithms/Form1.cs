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
    /// <summary>
    /// Главная форма приложения для визуализации генетического алгоритма размещения фигур.
    /// Отображает лучшую особь текущего поколения, позволяет добавлять круги и многоугольники,
    /// запускать поколения и наблюдать эволюцию.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly Random rnd = new();
        private readonly int width;
        private readonly int height;
        private readonly Population population;

        /// <summary>
        /// Инициализирует новую форму с заданными параметрами.
        /// </summary>
        /// <param name="width">Ширина области рисования (по умолчанию 400).</param>
        /// <param name="height">Высота области рисования (по умолчанию 400).</param>
        /// <param name="individuals">Количество особей в популяции (по умолчанию 80).</param>
        public Form1(int width = 400, int height = 400, int individuals = 80)
        {
            this.width = width;
            this.height = height;

            population = new Population(individuals, width, height);
            this.ClientSize = new Size(width + 150, height); // +150 для панели кнопок

            InitializeComponent();
            panel1.Size = new Size(width, height);
            this.DoubleBuffered = true; 

            population.InitializeRandom(); 

            panel1.Paint += panel1_Paint;
        }

        /// <summary>
        /// Обработчик события загрузки формы.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Обработчик события перерисовки панели.
        /// Рисует лучшую особь текущего поколения.
        /// </summary>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            Drawings.Draw(population.Best, e); // Выводим лучшую особь
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Следующее поколение".
        /// Запускает эволюцию, обновляет статистику и перерисовывает панель.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            button3.Visible = false;

            population.NextGeneration(mutationRate: 0.3);

            label1.Text = $"Generation: {population.Generation}";
            label2.Text = $"Best: {population.Best.Fitness():F2}";
            label3.Text = $"Avg: {population.AverageFitness():F2}";

            panel1.Invalidate(); // Перерисовка панели
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Добавить круг".
        /// Открывает диалоговое окно для ввода радиуса и добавляет круг в популяцию.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            using New_circle new_Circle = new New_circle();
            new_Circle.NewRadius += radius =>
            {
                Circle circle = new Circle(new SPoint(rnd.Next(width), rnd.Next(height)), radius);
                population.Add(circle);
            };

            new_Circle.ShowDialog();

            population.InitializeRandom(); // Перемешиваем позиции после добавления
            panel1.Invalidate();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Добавить многоугольник".
        /// Открывает диалоговое окно для ввода точек и добавляет многоугольник в популяцию.
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            using New_polygon new_Polygon = new New_polygon();
            new_Polygon.newPoints += points =>
            {
                Polygon polygon = new Polygon(points);
                population.Add(polygon);
            };

            new_Polygon.ShowDialog();

            population.InitializeRandom(); // Перемешиваем позиции
            panel1.Invalidate();
        }

        /// <summary>
        /// Пустой обработчик клика по метке поколения (оставлен для совместимости).
        /// </summary>
        private void label1_Click(object sender, EventArgs e)
        {
            // Можно добавить функциональность, например, сброс статистики
        }
    }
}