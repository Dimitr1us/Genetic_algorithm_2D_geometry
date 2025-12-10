using IndividualLib;
using iShapeLib;

namespace PopulationLib
{
    public class Population
    {
        private readonly Random rnd = new();
        private readonly List<Individual> individuals = new();
        public int Generation { get; private set; } = 0;
        public Individual Best => individuals.MaxBy(i => i.Fitness())!;

        public float AverageFitness() => individuals.Count == 0 ? 0f : individuals.Average(i => i.Fitness());

        public Population(int size, int width, int height)
        {
            for (int i = 0; i < size; i++)
                individuals.Add(new Individual(width, height));
        }

        public void Add(iShape prototype)
        {
            foreach (var ind in individuals)
                ind.Add(prototype);
        }

        public void InitializeRandom()
        {
            foreach (var ind in individuals)
                ind.Randomize();
        }

        public void NextGeneration(double mutationRate = 0.3, int tournamentSize = 5)
        {
            Generation++;
            var newGen = new List<Individual>();

            
            var sorted = individuals.OrderByDescending(x => x.Fitness()).ToList();
            newGen.Add(sorted[0].Clone());
            if (individuals.Count > 1) newGen.Add(sorted[1].Clone());

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

        public Individual this[int i] => individuals[i];

    }
}