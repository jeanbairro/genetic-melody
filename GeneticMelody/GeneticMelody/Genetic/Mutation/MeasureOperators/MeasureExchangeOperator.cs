using GeneticMelody.Converter;
using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System;
using System.Collections.Generic;
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

                while (indexB == indexA || !Valid(measure, indexA, indexB))
                {
                    indexA = ThreadSafeRandom.ThisThreadsRandom.Next(measure.Events.Count);
                    indexB = ThreadSafeRandom.ThisThreadsRandom.Next(measure.Events.Count);
                }

                // Before swap changes order
                var orderA = measure.Events[indexA].Order;
                measure.Events[indexA].Order = measure.Events[indexB].Order;
                measure.Events[indexB].Order = orderA;

                measure.Events = measure.Events.Swap(indexA, indexB).ToList();
            }
        }

        public bool Valid(Measure measure, int indexA, int indexB)
        {
            var events = measure.Events.ToList();
            events = events.Swap(indexA, indexB).ToList();

            for (int i = 0; i < events.Count; i++)
            {
                if (events[i].Number == (int)RestOrTie.Tie && i == 0)
                {
                    return false;
                }

                if (events[i].Number == (int)RestOrTie.Rest && i + 1 < events.Count && events[i + 1].Number == (int)RestOrTie.Tie)
                {
                    return false;
                }
            }

            return true;
        }
    }
}