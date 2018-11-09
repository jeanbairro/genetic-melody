using GeneticMelody.Genetic.Mutation.MeasureOperators;
using GeneticMelody.Genetic.Mutation.MelodyOperators;
using GeneticMelody.Genetic.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.Crossover
{
    public class RandomCutOffCrossoverOperator : ICrossoverOperator
    {
        private readonly List<IMeasureMutationOperator> _measureMutationOperators;
        private readonly List<IMelodyMutationOperator> _melodyMutationOperators;

        public RandomCutOffCrossoverOperator(List<IMeasureMutationOperator> measureMutationOperators, List<IMelodyMutationOperator> melodyMutationOperators)
        {
            _measureMutationOperators = measureMutationOperators;
            _melodyMutationOperators = melodyMutationOperators;
        }

        public Melody Cross(Melody firstParent, Melody secondParent)
        {
            var measuresCount = firstParent.Measures.Count;
            var numberOfFirstTake = ThreadSafeRandom.ThisThreadsRandom.Next(measuresCount + 1);
            var firstMeasures = firstParent.Measures.Take(numberOfFirstTake).ToList();
            var secondMeasures = secondParent.Measures.Skip(numberOfFirstTake).Take(measuresCount - numberOfFirstTake).ToList();
            var childMeasures = new List<Measure>();
            childMeasures.AddRange(firstMeasures);
            childMeasures.AddRange(secondMeasures);

            for (var i = 0; i < childMeasures.Count; i++)
            {
                _measureMutationOperators.ForEach(m => m.Mutate(childMeasures[i]));
                childMeasures[i].Order = i;
            }

            var melody = new Melody(childMeasures, firstParent.TimeMap);
            _melodyMutationOperators.ForEach(m => m.Mutate(melody));

            return melody;
        }
    }
}