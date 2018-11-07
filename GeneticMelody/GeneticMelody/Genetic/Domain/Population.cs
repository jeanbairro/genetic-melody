using GeneticMelody.Util;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.Domain
{
    public class Population
    {
        public Population(int number)
        {
            Individuals = new List<Melody>();
            Number = number;
        }

        public static int Limit => GeneticMelodyConstants.POPULATION_LIMIT;
        public ICollection<Melody> Individuals { get; set; }
        public int Number { get; set; }

        public double AverageFitness() => Individuals.Average(i => i.Fitness);

        public Melody BestIndividual()
        {
            return Individuals.OrderByDescending(i => i.Fitness).FirstOrDefault();
        }
    }
}