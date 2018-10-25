using GeneticMelody.Genetic.Domain;
using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System;
using System.Linq;

namespace GeneticMelody.Genetic.Mutation.MelodyOperators
{
    public class MelodicReplacementOperator : IMelodyReplacementOperator, IMutationOperator
    {
        public int Rate => GeneticMelodyConstants.DEFAULT_MUTATION_RATE;

        public void Mutate(Melody melody, Population population)
        {
            var randomRate = ThreadSafeRandom.ThisThreadsRandom.Next(0, 100);

            if (randomRate < Rate)
            {
                var allMeasures = population.Individuals.SelectMany(i => i.Measures).ToList();
                var indexOfRandomMeasureOfAll = ThreadSafeRandom.ThisThreadsRandom.Next(0, allMeasures.Count);
                var indexOfRandomMeasure = ThreadSafeRandom.ThisThreadsRandom.Next(0, melody.Measures.Count);
                var randomMeasureOfAll = allMeasures.ElementAt(indexOfRandomMeasureOfAll);
                melody.Measures[indexOfRandomMeasure].Events = randomMeasureOfAll.Events.ToList();
            }
        }
    }
}