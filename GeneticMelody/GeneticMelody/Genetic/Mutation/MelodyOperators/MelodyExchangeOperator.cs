using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System;
using System.Linq;

namespace GeneticMelody.Genetic.Mutation.MelodyOperators
{
    public class MelodyExchangeOperator : IMelodyMutationOperator, IMutationOperator
    {
        public int Rate => GeneticMelodyConstants.MUTATION_RATE_MELODY_EXCHANGE;

        public void Mutate(Melody melody)
        {
            var randomRate = ThreadSafeRandom.ThisThreadsRandom.Next(0, 100);

            if (randomRate < Rate)
            {
                var indexA = ThreadSafeRandom.ThisThreadsRandom.Next(melody.Measures.Count);
                var indexB = ThreadSafeRandom.ThisThreadsRandom.Next(melody.Measures.Count);

                while (indexB == indexA)
                {
                    indexB = ThreadSafeRandom.ThisThreadsRandom.Next(melody.Measures.Count);
                }

                // Before swap changes order
                var orderA = melody.Measures[indexA].Order;
                melody.Measures[indexA].Order = melody.Measures[indexB].Order;
                melody.Measures[indexB].Order = orderA;

                melody.Measures.Swap(indexA, indexB).ToList();
            }
        }
    }
}