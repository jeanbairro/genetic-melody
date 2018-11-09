using System;
using GeneticMelody.Genetic.Domain;
using GeneticMelody.Util;
using System.Linq;

namespace GeneticMelody.Genetic.StoppingCriterion
{
    public class MultipleStoppingCriterionChecker : IStoppingCriterionChecker
    {
        public bool Stop(MelodyGenerator melodyGenerator)
        {
            if (melodyGenerator.Generations.Last().BestIndividual().Fitness >= GeneticMelodyConstants.GREAT_FITNESS) return true;
            if (melodyGenerator.Generations.Count == GeneticMelodyConstants.GENERATIONS_LIMIT) return true;

            return false;
        }
    }
}