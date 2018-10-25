using GeneticMelody.Util;
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

        public static int SizeOfMeasure => GeneticMelodyConstants.SIZE_OF_MEASURE;
        public double DifferentIntervals => 0d;
        public IList<Event> Events { get; set; }
        public double NoteDensity => Events.OfType<Note>().Count() / Events.Count();
        public int Order { get; set; }
        public double PichVariety => Events.OfType<Note>().GroupBy(note => note.Number).Count() / Events.OfType<Note>().Count();
        public double RestDensity => Events.OfType<Rest>().Count() / Events.Count();
    }
}