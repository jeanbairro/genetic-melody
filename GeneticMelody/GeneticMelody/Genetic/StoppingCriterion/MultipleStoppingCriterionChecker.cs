using System;
using GeneticMelody.Genetic.Domain;
using GeneticMelody.Util;
using System.Linq;

namespace GeneticMelody.Genetic.StoppingCriterion
{
    public class MultipleStoppingCriterionChecker : IStoppingCriterionChecker
    {
        public bool Stop(Solver solver)
        {
            if (solver.Generations.Last().BestIndividual().Fitness < GeneticMelodyConstants.GREAT_FITNESS) return true;
            if (solver.Generations.Count == GeneticMelodyConstants.GENERATIONS_LIMIT) return true;

            return false;
        }
    }
}