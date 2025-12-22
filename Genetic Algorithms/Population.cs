using IndividualLib;
using iShapeLib;

namespace PopulationLib
{
    /// <summary>
    /// Представляет популяцию особей в генетическом алгоритме.
    /// Управляет поколениями, селекцией, кроссовером и мутацией.
    /// </summary>
    public class Population
    {
        private readonly Random rnd = new();
        private readonly List<Individual> individuals = new();

        /// <summary>
        /// Текущий номер поколения (начинается с 0).
        /// </summary>
        public int Generation { get; private set; } = 0;

        /// <summary>
        /// Лучшая особь в текущей популяции (с максимальной приспособленностью).
        /// </summary>
        /// <remarks>
        /// Если популяция пуста, возвращает <see langword="null"/> — но в нормальном состоянии популяция всегда содержит хотя бы одну особь.
        /// </remarks>
        public Individual Best => individuals.MaxBy(i => i.Fitness())!;

        /// <summary>
        /// Вычисляет среднюю приспособленность всех особей в популяции.
        /// </summary>
        /// <returns>Среднее значение <see cref="Individual.Fitness()"/> или 0, если популяция пуста.</returns>
        public float AverageFitness() => individuals.Count == 0 ? 0f : individuals.Average(i => i.Fitness());

        /// <summary>
        /// Создаёт новую популяцию заданного размера с пустыми особями.
        /// </summary>
        /// <param name="size">Количество особей в популяции.</param>
        /// <param name="width">Ширина поля (используется для инициализации каждой особи).</param>
        /// <param name="height">Высота поля (используется для инициализации каждой особи).</param>
        /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="size"/> меньше 1.</exception>
        public Population(int size, int width, int height)
        {
            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size), "Размер популяции должен быть не меньше 1.");

            for (int i = 0; i < size; i++)
                individuals.Add(new Individual(width, height));
        }

        /// <summary>
        /// Добавляет прототип фигуры (iShape) ко всем особям в популяции.
        /// </summary>
        /// <param name="prototype">Фигура-прототип, которую нужно добавить.</param>
        /// <remarks>Используется для добавления общей фигуры (например, начальной формы) ко всем особям.</remarks>
        public void Add(iShape prototype)
        {
            foreach (var ind in individuals)
                ind.Add(prototype);
        }

        /// <summary>
        /// Случайно инициализирует всех особей в популяции.
        /// </summary>
        /// <remarks>Вызывается обычно после создания популяции, чтобы заполнить её случайными данными.</remarks>
        public void InitializeRandom()
        {
            foreach (var ind in individuals)
                ind.Randomize();
        }

        /// <summary>
        /// Создаёт следующее поколение с помощью турнирной селекции, кроссовера и мутации.
        /// </summary>
        /// <param name="mutationRate">Вероятность мутации для каждого потомка (от 0.0 до 1.0). По умолчанию 0.3.</param>
        /// <param name="tournamentSize">Размер турнира для селекции (чем больше, тем сильнее давление отбора). По умолчанию 5.</param>
        /// <remarks>
        /// Гарантирует, что лучшее и второе лучшее решение переходят в следующее поколение без изменений (элитизм).
        /// </remarks>
        public void NextGeneration(double mutationRate = 0.3, int tournamentSize = 5)
        {
            Generation++;

            var newGen = new List<Individual>();

            var sorted = individuals.OrderByDescending(x => x.Fitness()).ToList();

            // Элитизм: сохраняем двух лучших
            newGen.Add(sorted[0].Clone());
            if (individuals.Count > 1)
                newGen.Add(sorted[1].Clone());

            while (newGen.Count < individuals.Count)
            {
                var p1 = TournamentSelect(tournamentSize);
                var p2 = TournamentSelect(tournamentSize);

                var child = p1.Crossover(p2);
                child.Mutate(mutationRate);
                newGen.Add(child);
            }

            individuals.Clear();
            individuals.AddRange(newGen);
        }

        /// <summary>
        /// Выбирает одну особь с помощью турнирной селекции.
        /// </summary>
        /// <param name="k">Размер турнира (количество кандидатов).</param>
        /// <returns>Особь с наивысшей приспособленностью из турнира.</returns>
        private Individual TournamentSelect(int k)
        {
            Individual best = individuals[rnd.Next(individuals.Count)];
            for (int i = 1; i < k; i++)
            {
                var cand = individuals[rnd.Next(individuals.Count)];
                if (cand.Fitness() > best.Fitness())
                    best = cand;
            }
            return best;
        }

        /// <summary>
        /// Получает особь по индексу.
        /// </summary>
        /// <param name="i">Индекс особи (от 0 до <see cref="individuals.Count"/> - 1).</param>
        /// <returns>Особь по указанному индексу.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Если индекс вне диапазона.</exception>
        public Individual this[int i] => individuals[i];
    }
}