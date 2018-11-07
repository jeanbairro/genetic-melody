using GeneticMelody.Converter;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.MusicTheory;

namespace GeneticMelody.Genetic
{
    public class Event
    {
        public Event()
        {
        }

        public Event(int number, int order)
        {
            Number = number;
            Order = order;
        }

        public int Number { get; set; }
        public int Order { get; set; }

        public Event Clone()
        {
            if (Number == (int)RestOrTie.Tie)
            {
                return new Tie(Number, Order);
            }

            if (Number == (int)RestOrTie.Rest)
            {
                return new Rest(Number, Order);
            }

            var name = NoteUtilities.GetNoteName((SevenBitNumber)Number).ToString();
            return new Note(name, Number, Order);
        }
    }
}