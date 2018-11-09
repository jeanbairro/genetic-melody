using GeneticMelody.Genetic.Util;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.Mutation.MeasureOperators
{
    public class MeasureInversionOperator : IMeasureMutationOperator, IMutationOperator
    {
        public MeasureInversionOperator(int rate)
        {
            Rate = rate;
        }

        public int Rate { get; set; }

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