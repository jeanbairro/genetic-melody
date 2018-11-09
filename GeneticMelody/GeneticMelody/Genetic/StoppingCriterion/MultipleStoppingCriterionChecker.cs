using System;
using GeneticMelody.Genetic.Domain;
using GeneticMelody.Util;
using System.Linq;

namespace GeneticMelody.Genetic.StoppingCriterion
{
    public class MultipleStoppingCriterionChecker : IStoppingCriterionChecker
    {
        public bool Stop(MelodyGenerator melodyGenerator, GeneticConfiguration configuration)
        {
            if (melodyGenerator.Generations.Last().BestIndividual().Fitness >= GeneticMelodyConstants.GREAT_FITNESS) return true;
            if (melodyGenerator.Generations.Count == configuration.GenerationLimit) return true;

            return false;
        }
    }
}