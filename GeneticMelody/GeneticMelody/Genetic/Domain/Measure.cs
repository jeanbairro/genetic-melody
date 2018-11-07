using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic
{
    public class Measure
    {
        public Measure(IList<Event> events, int order)
        {
            Events = events;
            Order = order;
        }

        public Measure()
        {
        }

        public double DifferentIntervals => 0d;
        public IList<Event> Events { get; set; }
        public double NoteDensity => Events.OfType<Note>().Count() / Events.Count();
        public int Order { get; set; }
        public double PichVariety => Events.OfType<Note>().GroupBy(note => note.Number).Count() / Events.OfType<Note>().Count();
        public double RestDensity => Events.OfType<Rest>().Count() / Events.Count();
        public int SizeOfMeasure { get; set; }

        public bool IsValid()
        {
            for (int i = 0; i < Events.Count; i++)
            {
                var measureEvent = Events[i];

                if (measureEvent is Tie && i == 0)
                {
                    return false;
                }

                if (measureEvent is Rest && i + 1 < Events.Count && Events[i + 1] is Tie)
                {
                    return false;
                }
            }

            return true;
        }
    }
}