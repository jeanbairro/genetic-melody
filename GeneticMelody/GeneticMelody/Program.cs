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
using GeneticMelody.Test;
using GeneticMelody.Util;
using Melanchall.DryWetMidi.Smf;
using System;
using System.Collections.Generic;

namespace GeneticMelody
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var converter = new MidiConverter();
            //var midi = MidiFile.Read(@"Files\Parabéns a Você.mid");
            //var input = converter.MidiToMelody(midi);
            //var inputToSave = converter.MelodyToMidi(input);
            //converter.SaveMidi(inputToSave, @"Files\input.mid");
            //var geneticConfiguration = new GeneticConfiguration { GenerationLimit = GeneticMelodyConstants.GENERATIONS_LIMIT, MutationRate = GeneticMelodyConstants.MUTATION_RATE_MEASURE_BROKER, PopulationLimit = GeneticMelodyConstants.POPULATION_LIMIT };

            //var measureMutationOperator = new List<IMeasureMutationOperator>
            //{
            //    new MeasureInversionOperator(geneticConfiguration.MutationRate),
            //    new MeasureReorganizationOperator(geneticConfiguration.MutationRate),
            //    new MeasureReplacementOperator(geneticConfiguration.MutationRate),
            //    new MeasureBrokerOperator(geneticConfiguration.MutationRate),
            //    new MeasureExchangeOperator(geneticConfiguration.MutationRate),
            //};

            //var melodyMutationOperators = new List<IMelodyMutationOperator>
            //{
            //    new MelodyExchangeOperator(geneticConfiguration.MutationRate),
            //    new MelodyInversionOperator(geneticConfiguration.MutationRate),
            //};
            //var crossoverOperator = new RandomCutOffCrossoverOperator(measureMutationOperator, melodyMutationOperators);
            //var initializer = new RandomInitializer(input);
            //var selector = new TournamentSelector();
            //var replacementOperator = new HalfReplacementOperator(crossoverOperator, selector, geneticConfiguration);
            //var fitnessCalculator = new MelodySimilarityFitnessCalculator();
            //var stoppingCriterionChecker = new MultipleStoppingCriterionChecker();
            //var geneticAlgorithm = new MelodyGenerator(crossoverOperator, fitnessCalculator, initializer, measureMutationOperator, melodyMutationOperators, replacementOperator, selector, stoppingCriterionChecker, geneticConfiguration);
            //var geneticEventsManager = Singleton<GeneticEventsManager>.Instance();
            //geneticEventsManager.Set(input);
            //// Test
            //new List<Melody>() { input }.ForEach(currentMelody => fitnessCalculator.Calculate(input, currentMelody));
            //var output = geneticAlgorithm.Generate();
            //var outputToSave = converter.MelodyToMidi(output);
            //converter.SaveMidi(outputToSave, @"Files\output.mid");

            var tester = new HeristicTest();
            tester.TestMutationRate();

            Console.ReadLine();
        }
    }
}