using GeneticMelody.Converter;
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
using Melanchall.DryWetMidi.Smf;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneticMelody.Test
{
    public class HeristicTest
    {
        public void TestPopSize()
        {
            var converter = new MidiConverter();
            var midi = MidiFile.Read(@"Files\Parabéns a Você.mid");
            var input = converter.MidiToMelody(midi);
            var geneticConfiguration = new GeneticConfiguration { GenerationLimit = 1000, MutationRate = 10, PopulationLimit = 10 };
            var measureMutationOperator = new List<IMeasureMutationOperator>
            {
                new MeasureInversionOperator(geneticConfiguration.MutationRate),
                new MeasureReorganizationOperator(geneticConfiguration.MutationRate),
                new MeasureReplacementOperator(geneticConfiguration.MutationRate),
                new MeasureBrokerOperator(geneticConfiguration.MutationRate),
                new MeasureExchangeOperator(geneticConfiguration.MutationRate),
            };

            var melodyMutationOperators = new List<IMelodyMutationOperator>
            {
                new MelodyExchangeOperator(geneticConfiguration.MutationRate),
                new MelodyInversionOperator(geneticConfiguration.MutationRate),
            };

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"GERACOES;TAMANHO_POP;");
            while (geneticConfiguration.PopulationLimit < 300)
            {
                var crossoverOperator = new RandomCutOffCrossoverOperator(measureMutationOperator, melodyMutationOperators);
                var initializer = new RandomInitializer(input);
                var selector = new TournamentSelector();
                var replacementOperator = new HalfReplacementOperator(crossoverOperator, selector, geneticConfiguration);
                var fitnessCalculator = new MelodySimilarityFitnessCalculator();
                var stoppingCriterionChecker = new MultipleStoppingCriterionChecker();
                var geneticAlgorithm = new MelodyGenerator(crossoverOperator, fitnessCalculator, initializer, measureMutationOperator, melodyMutationOperators, replacementOperator, selector, stoppingCriterionChecker, geneticConfiguration);
                var geneticEventsManager = Singleton<GeneticEventsManager>.Instance();
                geneticEventsManager.Set(input);
                var output = geneticAlgorithm.Generate();
                stringBuilder.AppendLine($"{geneticAlgorithm.Generations.Count};{geneticConfiguration.PopulationLimit};");
                System.Console.WriteLine($"{geneticAlgorithm.Generations.Count};{geneticConfiguration.PopulationLimit}");
                System.Console.WriteLine($"-------------------------");
                geneticConfiguration.PopulationLimit += 1;
            }

            File.WriteAllText(@"Files\PopSize.txt", stringBuilder.ToString());
        }

        public void TestMutationRate()
        {
            var converter = new MidiConverter();
            var midi = MidiFile.Read(@"Files\Parabéns a Você.mid");
            var input = converter.MidiToMelody(midi);
            var geneticConfiguration = new GeneticConfiguration { GenerationLimit = 500, MutationRate = 1, PopulationLimit = 50 };

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"GERACOES;TAMANHO_POP;MUTATION;AVG");
            while (geneticConfiguration.MutationRate < 100)
            {
                var measureMutationOperator = new List<IMeasureMutationOperator>
                {
                    new MeasureInversionOperator(geneticConfiguration.MutationRate),
                    new MeasureReorganizationOperator(geneticConfiguration.MutationRate),
                    new MeasureReplacementOperator(geneticConfiguration.MutationRate),
                    new MeasureBrokerOperator(geneticConfiguration.MutationRate),
                    new MeasureExchangeOperator(geneticConfiguration.MutationRate),
                };

                var melodyMutationOperators = new List<IMelodyMutationOperator>
                {
                    new MelodyExchangeOperator(geneticConfiguration.MutationRate),
                    new MelodyInversionOperator(geneticConfiguration.MutationRate),
                };

                var crossoverOperator = new RandomCutOffCrossoverOperator(measureMutationOperator, melodyMutationOperators);
                var initializer = new RandomInitializer(input);
                var selector = new TournamentSelector();
                var replacementOperator = new HalfReplacementOperator(crossoverOperator, selector, geneticConfiguration);
                var fitnessCalculator = new MelodySimilarityFitnessCalculator();
                var stoppingCriterionChecker = new MultipleStoppingCriterionChecker();
                var geneticAlgorithm = new MelodyGenerator(crossoverOperator, fitnessCalculator, initializer, measureMutationOperator, melodyMutationOperators, replacementOperator, selector, stoppingCriterionChecker, geneticConfiguration);
                var geneticEventsManager = Singleton<GeneticEventsManager>.Instance();
                geneticEventsManager.Set(input);
                var output = geneticAlgorithm.Generate();
                stringBuilder.AppendLine($"{geneticAlgorithm.Generations.Count};{geneticConfiguration.PopulationLimit};{geneticConfiguration.MutationRate};{geneticAlgorithm.Generations.SelectMany(x => x.Individuals).Average(i => i.Fitness)}");
                System.Console.WriteLine($"{geneticAlgorithm.Generations.Count};{geneticConfiguration.PopulationLimit};{geneticConfiguration.MutationRate};{geneticAlgorithm.Generations.SelectMany(x => x.Individuals).Average(i => i.Fitness)}");
                System.Console.WriteLine($"-------------------------");
                geneticConfiguration.MutationRate += 1;
            }

            File.WriteAllText(@"Files\MutationRate.txt", stringBuilder.ToString());
        }

        public void TestGenerationLimit()
        {
            var converter = new MidiConverter();
            var midi = MidiFile.Read(@"Files\Parabéns a Você.mid");
            var input = converter.MidiToMelody(midi);
        }
    }
}