using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.Mutation.MeasureOperators
{
    public class MeasureReorganizationOperator : IMeasureMutationOperator, IMutationOperator
    {
        public int Rate => GeneticMelodyConstants.DEFAULT_MUTATION_RATE;

        public void Mutate(Measure measure)
        {
            var randomRate = ThreadSafeRandom.ThisThreadsRandom.Next(0, 100);

            if (randomRate < Rate)
            {
                var notesReorganized = measure.Events.OfType<Note>().ToList().Shuffle();

                var newEvents = new List<Event>();
                var indexOfNotes = 0;
                Event currentEvent;
                for (int i = 0; i < measure.Events.Count; i++)
                {
                    if (measure.Events[i] is Note)
                    {
                        currentEvent = notesReorganized.ElementAt(indexOfNotes);
                        indexOfNotes++;
                    }
                    else
                    {
                        currentEvent = measure.Events[i];
                    }

                    currentEvent.Order = i;
                    newEvents.Add(currentEvent);
                }

                measure.Events = newEvents;
            }
        }
    }
}