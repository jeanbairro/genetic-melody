using GeneticMelody.Converter;
using GeneticMelody.Genetic.Domain;
using GeneticMelody.Genetic.Util;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.MusicTheory;

namespace GeneticMelody.Genetic.Mutation.MeasureOperators
{
    public class MeasureReplacementOperator : IMeasureMutationOperator, IMutationOperator
    {
        public MeasureReplacementOperator(int rate)
        {
            Rate = rate;
        }

        public int Rate { get; set; }

        public void Mutate(Measure measure)
        {
            var randomRate = ThreadSafeRandom.ThisThreadsRandom.Next(0, 100);

            if (randomRate < Rate)
            {
                var geneticEventsManager = Singleton<GeneticEventsManager>.Instance();
                var randomEvent = geneticEventsManager.RandomEventWithRate();
                var index = ThreadSafeRandom.ThisThreadsRandom.Next(measure.Events.Count);

                if (randomEvent == (int)RestOrTie.Rest)
                {
                    while (index + 1 < measure.Events.Count && measure.Events[index + 1].Number == (int)RestOrTie.Tie)
                    {
                        index = ThreadSafeRandom.ThisThreadsRandom.Next(measure.Events.Count);
                    }
                    measure.Events[index] = new Rest(randomEvent, index);
                    return;
                }

                if (randomEvent == (int)RestOrTie.Tie)
                {
                    while (index == 0 || (index - 1 >= 0 && measure.Events[index - 1].Number == (int)RestOrTie.Rest))
                    {
                        index = ThreadSafeRandom.ThisThreadsRandom.Next(measure.Events.Count);
                    }
                    measure.Events[index] = new Tie(randomEvent, index);
                    return;
                }

                var noteName = NoteUtilities.GetNoteName((SevenBitNumber)randomEvent).ToString();
                measure.Events[index] = new Note(noteName, randomEvent, index);
            }
        }
    }
}