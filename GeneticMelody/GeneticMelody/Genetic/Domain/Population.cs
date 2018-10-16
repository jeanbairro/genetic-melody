using GeneticMelody.Genetic.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;
using GeneticMelody.Util;

namespace GeneticMelody.Genetic.Domain
{
    public class Population : IPopulation
    {
        public Population()
        {
            Individuals = new List<Melody>();
        }

        public ICollection<Melody> Individuals { get; set; }

        public static int Limit => GeneticMelodyConstants.POPULATION_LIMIT;

        public IIndividual BestIndividual()
        {
            return Individuals.OrderByDescending(i => i.Fitness).FirstOrDefault();
        }
    }
}