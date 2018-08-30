using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using Melanchall.DryWetMidi.Tools;
using System.Linq;

namespace GeneticMelody
{
    class Program
    {
        static void Main(string[] args)
        {
            var midiFile = MidiFile.Read(@"MIDI Files\Parabéns a Você.mid");

            var differentNotes = midiFile.GetNotes().GroupBy(x => x.NoteName);

            var tempoMap = midiFile.GetTempoMap();

            //var timedEvents = midiFile.GetTimedEvents();

            //var teste = midiFile.SplitByNotes();

            var notesAndRests = midiFile.GetTrackChunks().Last().GetNotes().GetNotesAndRests(RestSeparationPolicy.NoSeparation); 

            var metricTime = TimeConverter.ConvertTo<MetricTimeSpan>(8, tempoMap);

            var barBeatTimeFromMetric = TimeConverter.ConvertTo<BarBeatTimeSpan>(metricTime, tempoMap);
            
            var teste = notesAndRests.AtTime(MusicalTimeSpan.Quarter, tempoMap);

            //midiFile.SplitNotesByGrid(new SteppedGrid());

            var newFiles = midiFile.SplitByGrid(new SteppedGrid(new BarBeatTimeSpan(1, 0, 0)));

            var i = 0;

            foreach (var midi in newFiles)
            {
                //midi.
                //midi.Write($@"MIDI Files\Teste{i}.mid");
                i++;
            }

            //System.Console.ReadKey();
        }
    }
}
