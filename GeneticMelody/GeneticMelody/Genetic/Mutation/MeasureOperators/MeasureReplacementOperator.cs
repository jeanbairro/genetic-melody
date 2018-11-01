using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System;

namespace GeneticMelody.Genetic.Mutation.MeasureOperators
{
    public class MeasureReplacementOperator : IMeasureMutationOperator, IMutationOperator
    {
        public int Rate => GeneticMelodyConstants.MUTATION_RATE_MEASURE_REPLACEMENT;

        public void Mutate(Measure measure)
        {
            throw new NotImplementedException();
        }
    }
}