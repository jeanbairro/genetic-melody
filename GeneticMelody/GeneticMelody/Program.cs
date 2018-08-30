using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using Melanchall.DryWetMidi.Tools;
using System;
using System.Linq;

namespace GeneticMelody
{
    class Program
    {
        static void Main(string[] args)
        {
            var midiFile = MidiFile.Read(@"MIDI Files\Parabéns a Você.mid");

            var differentNotes = midiFile.GetNotes().GroupBy(x => x.NoteName);

            differentNotes.ToList().ForEach(n => Console.WriteLine(n.Key));

            Print(midiFile);

            var tempoMap = midiFile.GetTempoMap();

            midiFile.QuantizeTimedEvents(new SteppedGrid(new BarBeatTimeSpan(1, tempoMap.TimeSignature.First().Value.Numerator, 8)), null);

            Print(midiFile);

            Console.ReadKey();
        }

        static void Print(MidiFile midiFile)
        {
            var midiMeasures = midiFile.SplitByGrid(new SteppedGrid(new BarBeatTimeSpan(1, 0, 0)));

            Console.WriteLine("-----------------------início----------------------");

            var i = 1;

            foreach (var midiMeasure in midiMeasures)
            {
                Console.WriteLine($"Measure {i}");
                var notesAndRests = midiMeasure.GetTrackChunks().Last().GetNotes().GetNotesAndRests(RestSeparationPolicy.NoSeparation);
                notesAndRests.ToList().ForEach(e => Console.WriteLine($"Event: {e.ToString()}, Time: {e.Time}, Length: {e.Length}"));
                Console.WriteLine("---------------------------------------------------");
                i++;
            }
        }
    }
}
