using GeneticMelody.Genetic.Domain;
using Melanchall.DryWetMidi.Smf.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic
{
    public class Melody : IIndividual
    {
        public Melody(ICollection<Measure> measures, TempoMap tempoMap)
        {
            Identity = Guid.NewGuid();
            Measures = measures;
            TempoMap = tempoMap;
        }

        public double DifferentIntervals => 0d;
        public double Fitness { get; set; }
        public Guid Identity { get; set; }
        public ICollection<Measure> Measures { get; set; }

        public double NoteDensity =>
            Measures.SelectMany(m => m.Events).OfType<Note>().Count() /
            Measures.SelectMany(m => m.Events).Count();

        public double PichVariety =>
            Measures.SelectMany(m => m.Events).OfType<Note>().GroupBy(note => note.Number).Count() /
            Measures.SelectMany(m => m.Events).OfType<Note>().Count();

        public double RestDensity =>
            Measures.SelectMany(m => m.Events).OfType<Rest>().Count() /
            Measures.SelectMany(m => m.Events).Count();

        public TempoMap TempoMap { get; set; }
    }
}