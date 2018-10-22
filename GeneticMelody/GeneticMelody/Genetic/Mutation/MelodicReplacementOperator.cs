using System;

namespace GeneticMelody.Genetic.Mutation
{
    public class MelodicReplacementOperator : IMelodyMutationOperator, IMutationOperator
    {
        public double Rate => 10;

        public void Mutate(Melody melody)
        {
            throw new NotImplementedException();
        }
    }
}