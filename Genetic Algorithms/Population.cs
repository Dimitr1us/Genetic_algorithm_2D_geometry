using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndividualLib;
using CrossShapesLib;
using iShapeLib;
using PolygonLib;
using CircleLib;
namespace PopulationLib
{
    internal class Population
    {
        private Random random = new Random();
        int number_individuals, number_in_group, round, width, height;
        List<Individual> individuals;
        List<float> fitnesses;
        public Population(int n, int k, int x, int y)
        {
            number_in_group = k;
            individuals = new List<Individual>();
            for (int i = 0; i < n; i++)
            {
                individuals.Add(new Individual(x, y,i));
            }
            number_individuals = n;
            width = x;
            height = y;
            round = 0;
        }

        public void Add(iShape shape)
        {
            if (shape is Circle circle)
            {
                foreach (Individual individual in individuals)
                {
                    individual.Add(circle.Clone());
                }
            }
            if (shape is Polygon polygon)
            {
                foreach (Individual individual in individuals)
                {
                    individual.Add(polygon.Clone());
                }
            }
        }

        public void Mix()
        {
            fitnesses = new List<float>();
            foreach (Individual individual in individuals)
            {
                individual.Mix();
                fitnesses.Add(individual.Fitness());
            }
        }

        public float Max()
        {
            return fitnesses.Max();
        }

        public Individual this[int index]
        {
            get => individuals[index];
        }

        public int GetRound()
        {
            return round;
        }

        public void Round(float probability_mutation = 0.1f)
        {
            round += 1;
            //int number_genes = random.Next(1,individuals[0].Count());

            List<Individual> notSelected = new List<Individual>(individuals);
            List<Individual> adapted = new List<Individual>();

            while (notSelected.Count > 0)
            {
                List<Individual> group = new List<Individual>();

                for (int i = 0; i < number_in_group && notSelected.Count > 0; i++)
                {
                    int idx = random.Next(notSelected.Count);
                    group.Add(notSelected[idx]);
                    notSelected.RemoveAt(idx);
                }

                Individual best = group
            .OrderByDescending(ind => fitnesses[ind.Id()])
            .First();

                adapted.Add(best);
            }

            for (int i = 0; i < adapted.Count-1; i = i + 2) {
                adapted[i].Intersection(adapted[i + 1], random.Next(individuals[0].Count()));
            }


            foreach (Individual individual in individuals)
            {
                if (random.NextDouble() <= probability_mutation)
                    individual.Mutation();
            }

            for (int i = 0; i < adapted.Count; i++)
            {
                fitnesses[adapted[i].Id()] = adapted[i].Fitness();
            }
        }
    }
}
