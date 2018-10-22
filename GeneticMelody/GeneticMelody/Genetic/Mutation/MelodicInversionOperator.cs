using GeneticMelody.Util;
using System;
using System.Linq;

namespace GeneticMelody.Genetic.Mutation
{
    public class MelodicInversionOperator : IMelodyMutationOperator, IMutationOperator
    {
        public int Rate => GeneticMelodyConstants.DEFAULT_MUTATION_RATE;

        public void Mutate(Melody melody)
        {
            var randomizer = new Random();
            var randomRate = randomizer.Next(0, 100);

            if (randomRate < Rate)
            {
                melody.Measures = melody.Measures.OrderByDescending(m => m.InitialOrder).ToList();
            }
        }
    }
}