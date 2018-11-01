using GeneticMelody.Genetic;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using Melanchall.DryWetMidi.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GeneticMelody.Converter
{
    public class MidiConverter
    {
        public MidiFile MelodyToMidi(Melody melody)
        {
            var tempoMap = melody.TempoMap;
            var ticksPerQuarterNote = ((TicksPerQuarterNoteTimeDivision)tempoMap.TimeDivision).TicksPerQuarterNote;
            var numberOfBeats = tempoMap.TimeSignature.FirstOrDefault()?.Value.Numerator ?? TimeSignature.Default.Numerator;
            var ticksPerEvent = ticksPerQuarterNote * numberOfBeats / Measure.SizeOfMeasure;
            var restTicks = 0;
            var tieTicks = 0;
            var events = new List<MidiEvent>
            {
                //new ProgramChangeEvent { ProgramNumber = (SevenBitNumber)100 }
            };

            var allEvents = melody.Measures.SelectMany(m => m.Events).ToList();
            for (var i = 0; i < allEvents.Count; i++)
            {
                var ev = allEvents[i];

                if (ev.Number == (int)RestOrTie.Rest)
                {
                    restTicks = ticksPerEvent;
                    continue;
                }

                while ((i + 1) < allEvents.Count && allEvents[i + 1].Number == (int)RestOrTie.Tie)
                {
                    tieTicks += ticksPerEvent;
                    i++;
                }

                events.Add(new NoteOnEvent { DeltaTime = 0 + restTicks, Channel = (FourBitNumber)0, NoteNumber = (SevenBitNumber)ev.Number, Velocity = (SevenBitNumber)127 });
                events.Add(new NoteOffEvent { DeltaTime = ticksPerEvent + tieTicks, Channel = (FourBitNumber)0, NoteNumber = (SevenBitNumber)ev.Number, Velocity = (SevenBitNumber)127 });
                restTicks = 0;
                tieTicks = 0;
            }

            var chunk = new TrackChunk(events);
            var midi = new MidiFile(new List<MidiChunk> { chunk });
            midi.ReplaceTempoMap(melody.TempoMap);
            return midi;
        }

        public Melody MidiToMelody(MidiFile midiFile)
        {
            var tempoMap = midiFile.GetTempoMap();
            var ticksPerQuarterNote = ((TicksPerQuarterNoteTimeDivision)tempoMap.TimeDivision).TicksPerQuarterNote;
            var numberOfBeats = tempoMap.TimeSignature.FirstOrDefault()?.Value.Numerator ?? TimeSignature.Default.Numerator;
            var ticksPerEvent = ticksPerQuarterNote * numberOfBeats / Measure.SizeOfMeasure;
            var midiMeasures = SplitMidiInMeasures(midiFile);

            var measures = new List<Measure>();
            foreach (var midiMeasure in midiMeasures)
            {
                measures.Add(MidiToMeasure(midiMeasure, measures.Count, ticksPerEvent));
            }

            return new Melody(measures, tempoMap.Clone());
        }

        public void Print(string nome, MidiFile midiFile)
        {
            var grid = new SteppedGrid(step: new BarBeatTimeSpan(bars: 1, beats: 0, ticks: 0));
            var midiMeasures = midiFile.SplitByGrid(grid, new SplittingMidiFileByGridSettings { RemoveEmptyFiles = true });

            Console.WriteLine("-----------------------início----------------------");

            var i = 1;

            foreach (var midiMeasure in midiMeasures)
            {
                var tempoMapMeasure = midiMeasure.GetTempoMap();

                Console.WriteLine($"Measure {i}");

                var notesAndRests = midiMeasure.GetTrackChunks().Last().GetNotes().GetNotesAndRests(RestSeparationPolicy.NoSeparation);

                notesAndRests.ToList().ForEach(e => Console.WriteLine($"Event: {e.ToString()}, Time: {e.Time}, Length: {e.Length}"));

                Console.WriteLine("---------------------------------------------------");

                if (File.Exists($@"Files\{nome}-{i}.mid"))
                {
                    File.Delete($@"Files\{nome}-{i}.mid");
                }

                midiMeasure.Write($@"Files\{nome}-{i}.mid");

                i++;
            }
        }

        public void SaveMidi(MidiFile midi, string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            midi.Write(path);
        }

        public void Teste()
        {
            var midiFile = MidiFile.Read(@"Files\teste.mid");

            var events = new List<ChannelEvent>();

            var ticks = 95;
            //for (int i = 0; i < 8; i++)
            //{
            //    events.Add(new NoteOnEvent { DeltaTime = ticks * i + i, Channel = (FourBitNumber)0, NoteNumber = (SevenBitNumber)(48 + i), Velocity = (SevenBitNumber)127 });
            //    events.Add(new NoteOffEvent { DeltaTime = ticks * (i + 1) + i, Channel = (FourBitNumber)0, NoteNumber = (SevenBitNumber)(48 + i), Velocity = (SevenBitNumber)127 });
            //}

            events.Add(new NoteOnEvent { DeltaTime = 0, Channel = (FourBitNumber)0, NoteNumber = (SevenBitNumber)(48), Velocity = (SevenBitNumber)127 });
            events.Add(new NoteOffEvent { DeltaTime = 96, Channel = (FourBitNumber)0, NoteNumber = (SevenBitNumber)(48), Velocity = (SevenBitNumber)127 });
            events.Add(new NoteOnEvent { DeltaTime = 0, Channel = (FourBitNumber)0, NoteNumber = (SevenBitNumber)(50), Velocity = (SevenBitNumber)127 });
            events.Add(new NoteOffEvent { DeltaTime = 96, Channel = (FourBitNumber)0, NoteNumber = (SevenBitNumber)(50), Velocity = (SevenBitNumber)127 });
            //events.Add(new NoteOnEvent { DeltaTime = 192, Channel = (FourBitNumber)0, NoteNumber = (SevenBitNumber)(52), Velocity = (SevenBitNumber)127 });
            //events.Add(new NoteOffEvent { DeltaTime = 288, Channel = (FourBitNumber)0, NoteNumber = (SevenBitNumber)(52), Velocity = (SevenBitNumber)127 });

            var chunk = new TrackChunk(events);
            var midiFile2 = new MidiFile(new List<MidiChunk> { chunk });
            midiFile2.Write($@"Files\midiFile2-teste-delta-time.mid");
            var tempomap2 = midiFile2.GetTempoMap();

            var tempoMap = midiFile.GetTempoMap();

            var differentNotes = midiFile2.GetNotes().GroupBy(x => x.NoteName);

            differentNotes.ToList().ForEach(n => Console.WriteLine(n.Key));

            Print("file", midiFile2);
        }

        private Event LengthedToEvent(ILengthedObject lengthed, int order)
        {
            if (lengthed is Melanchall.DryWetMidi.Smf.Interaction.Note note)
            {
                return new Genetic.Note(note.ToString(), note.NoteNumber, order);
            }

            return new Genetic.Rest((int)RestOrTie.Rest, order);
        }

        private Measure MidiToMeasure(MidiFile midiMeasure, int measureOrder, int ticksPerEvent)
        {
            var notesAndRests = midiMeasure.GetTrackChunks().Last().GetNotes().GetNotesAndRests(RestSeparationPolicy.NoSeparation);
            var measureTotalTicks = ticksPerEvent * Measure.SizeOfMeasure;
            var ticks = 0;

            var events = new List<Event>();
            while (ticks < measureTotalTicks)
            {
                Event measureEvent = null;
                var notesEndRestsOfTime = notesAndRests.AtTime(ticks).ToList();
                if (notesEndRestsOfTime.Any())
                {
                    measureEvent = LengthedToEvent(notesEndRestsOfTime.First(), events.Count);
                }
                else
                {
                    measureEvent = new Tie((int)RestOrTie.Tie, events.Count);
                }

                events.Add(measureEvent);
                ticks += ticksPerEvent;
            }

            return new Measure(events, measureOrder);
        }

        private ICollection<MidiFile> SplitMidiInMeasures(MidiFile midiFile)
        {
            var grid = new SteppedGrid(step: new BarBeatTimeSpan(bars: 1, beats: 0, ticks: 0));

            return midiFile.SplitByGrid(grid, new SplittingMidiFileByGridSettings { RemoveEmptyFiles = true }).ToList();
        }
    }
}