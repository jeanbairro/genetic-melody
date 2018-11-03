using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System.Linq;

namespace GeneticMelody.Genetic.Mutation.MeasureOperators
{
    public class MeasureExchangeOperator : IMeasureMutationOperator, IMutationOperator
    {
        public int Rate => GeneticMelodyConstants.MUTATION_RATE_MEASURE_EXCHANGE;

        public void Mutate(Measure measure)
        {
            var randomRate = ThreadSafeRandom.ThisThreadsRandom.Next(0, 100);

            if (randomRate < Rate)
            {
                var indexA = ThreadSafeRandom.ThisThreadsRandom.Next(measure.Events.Count);
                var indexB = ThreadSafeRandom.ThisThreadsRandom.Next(measure.Events.Count);

                while (indexB == indexA)
                {
                    indexB = ThreadSafeRandom.ThisThreadsRandom.Next(measure.Events.Count);
                }

                // Before swap changes order
                var orderA = measure.Events[indexA].Order;
                measure.Events[indexA].Order = measure.Events[indexB].Order;
                measure.Events[indexB].Order = orderA;

                measure.Events = measure.Events.Swap(indexA, indexB).ToList();
            }
        }
    }
}