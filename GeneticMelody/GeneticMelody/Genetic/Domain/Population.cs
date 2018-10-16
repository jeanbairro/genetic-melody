﻿using GeneticMelody.Util;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.Domain
{
    public class Population
    {
        public Population()
        {
            Individuals = new List<Melody>();
        }

        public ICollection<Melody> Individuals { get; set; }

        public static int Limit => GeneticMelodyConstants.POPULATION_LIMIT;

        public Melody BestIndividual()
        {
            return Individuals.OrderByDescending(i => i.Fitness).FirstOrDefault();
        }
    }
}