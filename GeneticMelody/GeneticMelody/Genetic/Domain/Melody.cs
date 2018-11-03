using GeneticMelody.Genetic.Domain;
using GeneticMelody.Util;
using Melanchall.DryWetMidi.Smf.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic
{
    public class Melody : IIndividual
    {
        public Melody(IList<Measure> measures, TempoMap tempoMap)
        {
            var numberOfBeats = tempoMap.TimeSignature.FirstOrDefault()?.Value.Numerator ?? TimeSignature.Default.Numerator;

            Identity = Guid.NewGuid();
            Measures = measures;
            TempoMap = tempoMap;
            SizeOfMeasure = (numberOfBeats * GeneticMelodyConstants.SLOTS_PER_BEAT);
        }

        public Melody()
        {
        }

        public double Fitness { get; set; }
        public Guid Identity { get; set; }
        public IList<Measure> Measures { get; set; }
        public int SizeOfMeasure { get; set; }
        public TempoMap TempoMap { get; set; }

        public Melody Clone()
        {
            return new Melody
            {
                Fitness = this.Fitness,
                Identity = this.Identity,
                Measures = this.Measures.Select(m => new Measure
                {
                    Events = m.Events.Select(e => new Event
                    {
                        Number = e.Number,
                        Order = e.Order,
                    }).ToList(),
                    Order = m.Order
                }).ToList(),
                SizeOfMeasure = this.SizeOfMeasure,
                TempoMap = this.TempoMap
            };
        }
    }
}