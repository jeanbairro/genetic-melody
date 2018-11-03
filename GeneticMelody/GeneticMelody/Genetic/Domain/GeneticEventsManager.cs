using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.Domain
{
    public class GeneticEventsManager
    {
        private List<int> DifferentEvents { get; set; }

        public void SetEvents(Melody baseMelody)
        {
            if (this.DifferentEvents == null)
            {
                this.DifferentEvents = new List<int>();
            }

            var differentEvents = baseMelody
                .Measures
                .SelectMany(m => m.Events);

            if (GeneticMelodyConstants.KEEP_ORIGINAL_TIES)
            {
                differentEvents = differentEvents.Where(e => e is Note || e is Rest);
            }

            var grouped = differentEvents.GroupBy(e => e.Number).ToList();

            this.DifferentEvents.AddRange(grouped.Select((IGrouping<int, Event> e) => e.Key));
        }

        public int RandomEvent()
        {
            var index = ThreadSafeRandom.ThisThreadsRandom.Next(DifferentEvents.Count);
            return DifferentEvents[index];
        }
    }
}