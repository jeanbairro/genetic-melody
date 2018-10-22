using GeneticMelody.Genetic.Domain;
using Melanchall.DryWetMidi.Smf.Interaction;
using System;
using System.Collections.Generic;

namespace GeneticMelody.Genetic
{
    public class Melody : IIndividual
    {
        public Melody(IList<Measure> measures, TempoMap tempoMap)
        {
            Identity = Guid.NewGuid();
            Measures = measures;
            TempoMap = tempoMap;
        }

        public double Fitness { get; set; }
        public Guid Identity { get; set; }
        public IList<Measure> Measures { get; set; }
        public TempoMap TempoMap { get; set; }
    }
}