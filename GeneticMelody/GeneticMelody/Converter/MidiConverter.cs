using GeneticMelody.Genetic;
using GeneticMelody.Genetic.Util;
using GeneticMelody.Util;
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
            var tempoMap = melody.TimeMap;
            var ticksPerQuarterNote = ((TicksPerQuarterNoteTimeDivision)tempoMap.TimeDivision).TicksPerQuarterNote;
            var numberOfBeats = tempoMap.TimeSignature.FirstOrDefault()?.Value.Numerator ?? TimeSignature.Default.Numerator;
            var ticksPerEvent = ticksPerQuarterNote * numberOfBeats / melody.SizeOfMeasure;
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
            midi.ReplaceTempoMap(melody.TimeMap);
            return midi;
        }

        public Melody MidiToMelody(MidiFile midiFile)
        {
            var tempoMap = midiFile.GetTempoMap();
            var ticksPerQuarterNote = ((TicksPerQuarterNoteTimeDivision)tempoMap.TimeDivision).TicksPerQuarterNote;
            var numberOfBeats = tempoMap.TimeSignature.FirstOrDefault()?.Value.Numerator ?? TimeSignature.Default.Numerator;
            var sizeOfMeasure = numberOfBeats * GeneticMelodyConstants.SLOTS_PER_BEAT;
            var ticksPerEvent = ticksPerQuarterNote * numberOfBeats / sizeOfMeasure;
            var midiMeasures = SplitMidiInMeasures(midiFile);

            var measures = new List<Measure>();
            foreach (var midiMeasure in midiMeasures)
            {
                measures.Add(MidiToMeasure(midiMeasure, measures.Count, ticksPerEvent, sizeOfMeasure));
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

        private Event LengthedToEvent(ILengthedObject lengthed, int order)
        {
            if (lengthed is Melanchall.DryWetMidi.Smf.Interaction.Note note)
            {
                return new Genetic.Note(note.ToString(), note.NoteNumber, order);
            }

            return new Genetic.Rest((int)RestOrTie.Rest, order);
        }

        private Measure MidiToMeasure(MidiFile midiMeasure, int measureOrder, int ticksPerEvent, int sizeOfMeasure)
        {
            var notesAndRests = midiMeasure.GetTrackChunks().Last().GetNotes().GetNotesAndRests(RestSeparationPolicy.NoSeparation).ToList();
            var measureTotalTicks = ticksPerEvent * sizeOfMeasure;
            var ticks = 0;

            var events = new List<Event>();
            ILengthedObject previous = null;
            while (ticks < measureTotalTicks)
            {
                Event measureEvent = null;
                var notesEndRestsOfTime = notesAndRests.PlayingAtTime(ticks);
                if (notesEndRestsOfTime.Any())
                {
                    var lengthed = notesEndRestsOfTime.First();

                    if (lengthed is Melanchall.DryWetMidi.Smf.Interaction.Rest || !lengthed.Equal(previous))
                    {
                        measureEvent = LengthedToEvent(lengthed, events.Count);
                        previous = lengthed;
                    }
                    else
                    {
                        measureEvent = new Tie((int)RestOrTie.Tie, events.Count);
                    }
                }
                else
                {
                    measureEvent = new Genetic.Rest((int)RestOrTie.Rest, events.Count);
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