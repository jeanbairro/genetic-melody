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
using Melanchall.DryWetMidi.Smf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GeneticMelody
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var input = MidiFile.Read(@"Files\teste.mid");
            var melody = new MidiConverter().MidiToMelody(input);

            Print(melody);

            var measureMutationOperator = new List<IMeasureMutationOperator>
            {
                new MeasureExchangeOperator(),
                new MeasureInversionOperator(),
                new MeasureReorganizationOperator()
            };

            var melodyMutationOperators = new List<IMelodyMutationOperator>
            {
                new MelodicExchangeOperator(),
                new MelodicInversionOperator(),
            };

            var crossoverOperator = new RandomCutOffCrossoverOperator(measureMutationOperator, melodyMutationOperators);
            var initializer = new RandomInitializer(melody);
            var selector = new TournamentSelector();
            var replacementOperator = new HalfReplacementOperator(crossoverOperator, selector);
            var fitnessCalculator = new MelodySimilarityFitnessCalculator();
            var stoppingCriterionChecker = new MultipleStoppingCriterionChecker();
            var geneticAlgorithm = new Solver(crossoverOperator, fitnessCalculator, initializer, replacementOperator, selector, stoppingCriterionChecker);
            var output = geneticAlgorithm.Solve();
            Print(output);

            Console.ReadKey();
        }

        private static void Print(Melody melody)
        {
            Console.WriteLine($"Fitness: {melody.Fitness}");
            foreach (var measure in melody.Measures)
            {
                Console.WriteLine($"Measure: {measure.Order}");
                measure.Events.ToList().ForEach(e => Console.Write($"{e.Number} "));
                Console.WriteLine("");
            }
            Console.WriteLine("---------------------------------------------------------");
        }
    }
}