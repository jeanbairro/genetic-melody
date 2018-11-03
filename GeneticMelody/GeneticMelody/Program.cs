using GeneticMelody.Converter;
using GeneticMelody.Genetic;
using GeneticMelody.Genetic.Crossover;
using GeneticMelody.Genetic.Domain;
using GeneticMelody.Genetic.FitnessCalculators;
using GeneticMelody.Genetic.Initialization;
using GeneticMelody.Genetic.Mutation.MeasureOperators;
using GeneticMelody.Genetic.Mutation.MelodyOperators;
using GeneticMelody.Genetic.Replacement;
using GeneticMelody.Genetic.Selection;
using GeneticMelody.Genetic.StoppingCriterion;
using GeneticMelody.Genetic.Util;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Smf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var converter = new MidiConverter();
            var midi = MidiFile.Read(@"Files\Parabéns a Você.mid");
            var input = converter.MidiToMelody(midi);
            Print(input);
            var inputToSave = converter.MelodyToMidi(input);
            converter.SaveMidi(inputToSave, @"Files\input.mid");

            var measureMutationOperator = new List<IMeasureMutationOperator>
            {
                new MeasureExchangeOperator(),
                new MeasureInversionOperator(),
                new MeasureReorganizationOperator(),
                new MeasureReplacementOperator(),
            };

            var melodyMutationOperators = new List<IMelodyMutationOperator>
            {
                new MelodicExchangeOperator(),
                new MelodicInversionOperator(),
            };

            var crossoverOperator = new RandomCutOffCrossoverOperator(measureMutationOperator, melodyMutationOperators);
            var initializer = new RandomInitializer(input);
            var selector = new TournamentSelector();
            var replacementOperator = new HalfReplacementOperator(crossoverOperator, selector);
            var fitnessCalculator = new MelodySimilarityFitnessCalculator();
            var stoppingCriterionChecker = new MultipleStoppingCriterionChecker();
            var geneticAlgorithm = new Solver(crossoverOperator, fitnessCalculator, initializer, replacementOperator, selector, stoppingCriterionChecker);
            var geneticEventsManager = Singleton<GeneticEventsManager>.Instance();
            geneticEventsManager.SetEvents(input);
            var output = geneticAlgorithm.Solve();
            Print(output);
            var outputToSave = converter.MelodyToMidi(output);
            converter.SaveMidi(outputToSave, @"Files\output.mid");

            Console.ReadKey();
        }

        private static void Print(Melody melody)
        {
            Console.SetWindowSize(Console.LargestWindowWidth - 20, Console.LargestWindowHeight);
            Console.Write($"Fitness: {melody.Fitness}\n\n");
            foreach (var measure in melody.Measures)
            {
                Console.Write($"Measure {measure.Order.ToString("00")}: ");
                measure.Events.ToList().ForEach(e => Console.Write($"{PrintEvent(e.Number)} "));
                Console.Write("\n\n");
            }
            Console.WriteLine("---------------------------------------------------------");
        }

        private static string PrintEvent(int number)
        {
            if (number == (int)RestOrTie.Rest)
            {
                return "REST";
            }

            if (number == (int)RestOrTie.Tie)
            {
                return "SUST";
            }

            return NoteUtilities.GetNoteName((SevenBitNumber)number).ToString();
        }
    }
}