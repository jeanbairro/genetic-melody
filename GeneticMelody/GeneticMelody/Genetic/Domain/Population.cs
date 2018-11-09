using GeneticMelody.Util;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.Domain
{
    public class Population
    {
        public Population(int sequence)
        {
            Individuals = new List<Melody>();
            Sequence = sequence;
        }

        public static int Limit => GeneticMelodyConstants.POPULATION_LIMIT;
        public ICollection<Melody> Individuals { get; set; }
        public int Sequence { get; set; }

        public double AverageFitness() => Individuals.Average(i => i.Fitness);

        public Melody BestIndividual()
        {
            return Individuals.OrderByDescending(i => i.Fitness).FirstOrDefault();
        }
    }
}