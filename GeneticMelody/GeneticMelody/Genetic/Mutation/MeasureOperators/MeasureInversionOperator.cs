using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System;
using System.Linq;

namespace GeneticMelody.Genetic.Mutation.MeasureOperators
{
    public class MeasureInversionOperator : IMeasureMutationOperator, IMutationOperator
    {
        public int Rate => GeneticMelodyConstants.MUTATION_RATE_MEASURE_INVERSION;

        public void Mutate(Measure measure)
        {
            var randomRate = ThreadSafeRandom.ThisThreadsRandom.Next(0, 100);

            if (randomRate < Rate)
            {
                var events = measure.Events.OrderByDescending(m => m.Order).ToList();
                var i = 0;
                events.ForEach(e =>
                {
                    e.Order = i++;
                });
                measure.Events = events;
            }
        }
    }
}