using GeneticMelody.Converter;
using GeneticMelody.Genetic.Domain;
using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.MusicTheory;
using System;

namespace GeneticMelody.Genetic.Mutation.MeasureOperators
{
    public class MeasureReplacementOperator : IMeasureMutationOperator, IMutationOperator
    {
        public int Rate => GeneticMelodyConstants.MUTATION_RATE_MEASURE_REPLACEMENT;

        public void Mutate(Measure measure)
        {
            var randomRate = ThreadSafeRandom.ThisThreadsRandom.Next(0, 100);

            if (randomRate < Rate)
            {
                var geneticEventsManager = Singleton<GeneticEventsManager>.Instance();
                var randomEvent = geneticEventsManager.RandomEvent();
                var index = ThreadSafeRandom.ThisThreadsRandom.Next(measure.Events.Count);

                if (randomEvent == (int)RestOrTie.Rest)
                {
                    measure.Events[index] = new Rest(randomEvent, index);
                    return;
                }

                if (randomEvent == (int)RestOrTie.Tie)
                {
                    measure.Events[index] = new Tie(randomEvent, index);
                    return;
                }

                var noteName = NoteUtilities.GetNoteName((SevenBitNumber)randomEvent).ToString();
                measure.Events[index] = new Note(noteName, randomEvent, index);
            }
        }
    }
}