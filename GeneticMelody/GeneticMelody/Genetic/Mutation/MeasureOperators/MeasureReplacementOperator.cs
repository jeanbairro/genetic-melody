using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System;

namespace GeneticMelody.Genetic.Mutation.MeasureOperators
{
    public class MeasureReplacementOperator : IMeasureMutationOperator, IMutationOperator
    {
        public int Rate => GeneticMelodyConstants.DEFAULT_MUTATION_RATE;

        public void Mutate(Measure measure)
        {
            throw new NotImplementedException();
        }
    }
}