using GeneticMelody.Genetic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.Initialization
{
    public class RandomInitializer : IInitializazer
    {
        public RandomInitializer(Melody melody)
        {
            BaseMelody = melody;
        }

        public Melody BaseMelody { get; set; }

        public Population Initialize()
        {
            var population = new Population();

            while (population.Individuals.Count < Population.Limit)
            {
                population.Individuals.Add(new Melody(GetMeasures(), BaseMelody.TempoMap));
            }

            return population;
        }

        private ICollection<Measure> GetMeasures()
        {
            var measures = new List<Measure>();

            while (measures.Count < BaseMelody.Measures.Count)
            {
                measures.Add(new Measure(RandomizeEvents()));
            }

            return measures;
        }

        private ICollection<Event> RandomizeEvents()
        {
            var events = new List<Event>();
            var differentEvents = BaseMelody.Measures.SelectMany(m => m.Events).GroupBy(e => e.Number).ToList();
            var randomizer = new Random();

            while (events.Count < Measure.SizeOfMeasure)
            {
                var index = randomizer.Next(differentEvents.Count);
                events.Add(new Event(differentEvents[index].Key));
            }

            return events;
        }
    }
}