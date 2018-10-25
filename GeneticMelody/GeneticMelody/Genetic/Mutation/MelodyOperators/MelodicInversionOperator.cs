using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System;
using System.Linq;

namespace GeneticMelody.Genetic.Mutation.MelodyOperators
{
    public class MelodicInversionOperator : IMelodyMutationOperator, IMutationOperator
    {
        public int Rate => GeneticMelodyConstants.DEFAULT_MUTATION_RATE;

        public void Mutate(Melody melody)
        {
            var randomRate = ThreadSafeRandom.ThisThreadsRandom.Next(0, 100);

            if (randomRate < Rate)
            {
                var measures = melody.Measures.OrderByDescending(m => m.Order).ToList();
                var i = 0;
                measures.ForEach(e =>
                {
                    e.Order = i++;
                });
                melody.Measures = measures;
            }
        }
    }
}