using GeneticMelody.Util;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic
{
    public class Measure
    {
        public Measure(ICollection<Event> events)
        {
            Events = events;
        }

        public ICollection<Event> Events { get; set; }

        public static int SizeOfMeasure => GeneticMelodyConstants.SIZE_OF_MEASURE;

        public double PichVariety => Events.OfType<Note>().GroupBy(note => note.Number).Count() / Events.OfType<Note>().Count();

        public double DifferentIntervals => 0d;

        public double NoteDensity => Events.OfType<Note>().Count() / Events.Count();

        public double RestDensity => Events.OfType<Rest>().Count() / Events.Count();
    }
}