using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System;

namespace GeneticMelody.Genetic.Mutation
{
    public class MelodicExchangeOperator : IMelodyMutationOperator, IMutationOperator
    {
        public int Rate => GeneticMelodyConstants.DEFAULT_MUTATION_RATE;

        public void Mutate(Melody melody)
        {
            var randomizer = new Random();
            var randomRate = randomizer.Next(0, 100);

            if (randomRate < Rate)
            {
                var indexA = randomizer.Next(melody.Measures.Count);
                var indexB = randomizer.Next(melody.Measures.Count);

                while (indexB == indexA)
                {
                    indexB = randomizer.Next(melody.Measures.Count);
                }

                melody.Measures = melody.Measures.Swap(indexA, indexB);
            }
        }
    }
}