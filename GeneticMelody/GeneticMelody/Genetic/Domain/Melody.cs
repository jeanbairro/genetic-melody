using GeneticMelody.Genetic.Domain.Interfaces;
using Melanchall.DryWetMidi.Smf.Interaction;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic
{
    public class Melody : IIndividual
    {
        public Melody(ICollection<Measure> measures, TempoMap tempoMap)
        {
            Measures = measures;
            TempoMap = tempoMap;
        }

        public double Fitness { get; set; }

        public TempoMap TempoMap { get; set; }

        public ICollection<Measure> Measures { get; set; }

        public double PichVariety =>
            Measures.SelectMany(m => m.Events).OfType<Note>().GroupBy(note => note.Number).Count() /
            Measures.SelectMany(m => m.Events).OfType<Note>().Count();

        public double DifferentIntervals => 0d;

        public double NoteDensity =>
            Measures.SelectMany(m => m.Events).OfType<Note>().Count() /
            Measures.SelectMany(m => m.Events).Count();

        public double RestDensity =>
            Measures.SelectMany(m => m.Events).OfType<Rest>().Count() /
            Measures.SelectMany(m => m.Events).Count();
    }
}