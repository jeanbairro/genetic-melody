using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System;
using System.Linq;

namespace GeneticMelody.Genetic.Mutation.MelodyOperators
{
    public class MelodyInversionOperator : IMelodyMutationOperator, IMutationOperator
    {
        public int Rate => GeneticMelodyConstants.MUTATION_RATE_MELODY_INVERSION;

        public void Mutate(Melody melody)
        {
            var randomRate = ThreadSafeRandom.ThisThreadsRandom.Next(0, 100);

            if (randomRate < Rate)
            {
                var measures = melody.Measures.OrderByDescending(m => m.Order).ToList();
                for (var i = 0; i < measures.Count; i++)
                {
                    measures[i].Order = i;
                }

                melody.Measures = measures.ToList();
            }
        }
    }
}