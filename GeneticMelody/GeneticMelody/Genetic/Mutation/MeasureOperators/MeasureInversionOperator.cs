using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System.Collections.Generic;
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
                var invertedNotes = measure.Events.OfType<Note>().OrderByDescending(m => m.Order).ToList();
                var newEvents = new List<Event>();
                var indexOfNotes = 0;
                Event currentEvent;
                for (int i = 0; i < measure.Events.Count; i++)
                {
                    if (measure.Events[i] is Note)
                    {
                        currentEvent = invertedNotes.ElementAt(indexOfNotes);
                        indexOfNotes++;
                    }
                    else
                    {
                        currentEvent = measure.Events[i];
                    }

                    currentEvent.Order = i;
                    newEvents.Add(currentEvent);
                }
            }
        }
    }
}