using GeneticMelody.Converter;
using GeneticMelody.Genetic.Domain;
using GeneticMelody.Genetic.GeneticOperators.Initializer;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using Melanchall.DryWetMidi.Tools;
using System;
using System.IO;
using System.Linq;

namespace GeneticMelody
{
    class Program
    {
        static void Main(string[] args)
        {
            var midiFile = MidiFile.Read(@"Files\teste.mid");

            var converter = new MidiConverter();

            var initializer = new RandomInitializer(converter.MidiToMelody(midiFile));

            var firstPopulation = initializer.Initialize();

            firstPopulation.BestIndividual();

            Console.ReadKey();
        }
    }
}
