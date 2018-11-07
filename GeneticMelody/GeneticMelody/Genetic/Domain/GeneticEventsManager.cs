using GeneticMelody.Converter;
using GeneticMelody.Genetic.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.Domain
{
    public class GeneticEventsManager
    {
        public GeneticEventsManager()
        {
            DifferentEvents = new List<int>();
            Rates = new List<TypeEventRate>();
        }

        private List<int> DifferentEvents { get; set; }
        private List<TypeEventRate> Rates { get; set; }

        public int RandomEventWithRate()
        {
            var type = GetTypeByRate();

            if (type == typeof(Note))
            {
                var differentNotes = DifferentEvents.Where(e => e != (int)RestOrTie.Tie && e != (int)RestOrTie.Rest).ToList();
                var index = ThreadSafeRandom.ThisThreadsRandom.Next(differentNotes.Count);
                return differentNotes[index];
            }
            else if (type == typeof(Rest))
            {
                return (int)RestOrTie.Rest;
            }
            else
            {
                return (int)RestOrTie.Tie;
            }
        }

        public int RandomEvent()
        {
            var index = ThreadSafeRandom.ThisThreadsRandom.Next(DifferentEvents.Count);
            return DifferentEvents[index];
        }

        public int RandomNote()
        {
            var differentEvents = DifferentEvents.Where(e => e != (int)RestOrTie.Tie && e != (int)RestOrTie.Rest).ToList();
            var index = ThreadSafeRandom.ThisThreadsRandom.Next(differentEvents.Count);
            return differentEvents[index];
        }

        public int RandomRestOrNote()
        {
            var differentEvents = DifferentEvents.Where(e => e != (int)RestOrTie.Tie).ToList();
            var index = ThreadSafeRandom.ThisThreadsRandom.Next(differentEvents.Count);
            return differentEvents[index];
        }

        public void Set(Melody baseMelody)
        {
            var allEvents = baseMelody
                .Measures
                .SelectMany(m => m.Events).ToList();

            var groupedByType = allEvents.GroupBy(e => e.Number).ToList();

            DifferentEvents.AddRange(groupedByType.Select((IGrouping<int, Event> e) => e.Key));
            Rates.Add(new TypeEventRate(typeof(Note), allEvents.OfType<Note>().Count() / (double)allEvents.Count() * 100));
            Rates.Add(new TypeEventRate(typeof(Rest), allEvents.OfType<Rest>().Count() / (double)allEvents.Count() * 100));
            Rates.Add(new TypeEventRate(typeof(Tie), allEvents.OfType<Tie>().Count() / (double)allEvents.Count() * 100));
            Rates.OrderBy(r => r.Rate);
        }

        private Type GetTypeByRate()
        {
            var randomRate = ThreadSafeRandom.ThisThreadsRandom.Next(0, 100);

            for (var i = 0; i < Rates.Count; i++)
            {
                var rate = Rates[i].Rate;

                if (i > 0)
                {
                    rate = Rates[i].Rate + Rates[i - 1].Rate;
                }

                if (randomRate < rate)
                {
                    return Rates[i].Type;
                }
            }

            return null;
        }
    }
}