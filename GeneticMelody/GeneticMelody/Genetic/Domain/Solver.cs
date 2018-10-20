using GeneticMelody.Converter;
using GeneticMelody.Genetic.Initialization;
using Melanchall.DryWetMidi.Smf;
using System.Collections.Generic;

namespace GeneticMelody.Genetic.Domain
{
    public class Solver
    {
        public ICollection<Population> Generations { get; set; }

        public MidiFile Solve(MidiFile midiFile)
        {
            var converter = new MidiConverter();

            var initializer = new RandomInitializer(converter.MidiToMelody(midiFile));

            var genesisPopulation = initializer.Initialize();

            /*aplicar operadores geneticos*/

            Generations.Add(genesisPopulation);

            while (/*criterio de parada não for atingido*/2 == 1)
            {
                /*dale o*/
            }

            return new MidiFile();
        }
    }
}