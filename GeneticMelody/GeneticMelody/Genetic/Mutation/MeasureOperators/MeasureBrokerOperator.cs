using GeneticMelody.Converter;
using GeneticMelody.Genetic.Domain;
using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.MusicTheory;
using System.Linq;

namespace GeneticMelody.Genetic.Mutation.MeasureOperators
{
    public class MeasureBrokerOperator : IMeasureMutationOperator, IMutationOperator
    {
        public MeasureBrokerOperator(int rate)
        {
            Rate = rate;
        }

        public int Rate { get; set; }

        public void Mutate(Measure measure)
        {
            if (!measure.Events.OfType<Rest>().Any())
            {
                return;
            }

            var randomRate = ThreadSafeRandom.ThisThreadsRandom.Next(0, 100);

            if (randomRate < Rate)
            {
                var geneticEventsManager = Singleton<GeneticEventsManager>.Instance();

                for (int i = 0; i < measure.Events.Count; i++)
                {
                    if (measure.Events[i].Number == (int)RestOrTie.Rest && i + 1 < measure.Events.Count && measure.Events[i + 1].Number == (int)RestOrTie.Tie)
                    {
                        var randomNote = geneticEventsManager.RandomNote();
                        var noteName = NoteUtilities.GetNoteName((SevenBitNumber)randomNote).ToString();
                        measure.Events[i] = new Note(noteName, randomNote, i);
                    }
                }
            }
        }
    }
}