using GeneticMelody.Converter;
using GeneticMelody.Genetic.Crossover;
using GeneticMelody.Genetic.FitnessCalculators;
using GeneticMelody.Genetic.Initialization;
using GeneticMelody.Genetic.Mutation.MeasureOperators;
using GeneticMelody.Genetic.Mutation.MelodyOperators;
using GeneticMelody.Genetic.Replacement;
using GeneticMelody.Genetic.Selection;
using GeneticMelody.Genetic.StoppingCriterion;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.Domain
{
    public class Solver
    {
        private readonly ICrossoverOperator _crossoverOperator;
        private readonly ISimilarityFitnessCalculator _fitnessCalculator;
        private readonly IInitializazer _initializer;
        private readonly List<IMeasureMutationOperator> _measureMutationOperators;
        private readonly List<IMelodyMutationOperator> _melodyMutationOperators;
        private readonly IReplacementOperator _replacementOperator;
        private readonly ISelector _selector;
        private readonly IStoppingCriterionChecker _stopChecker;

        public Solver(ICrossoverOperator crossoverOperator, ISimilarityFitnessCalculator fitnessCalculator, IInitializazer initializer, List<IMeasureMutationOperator> measureMutationOperators, List<IMelodyMutationOperator> melodyMutationOperators, IReplacementOperator replacementOperator, ISelector selector, IStoppingCriterionChecker stopChecker)
        {
            _crossoverOperator = crossoverOperator;
            _fitnessCalculator = fitnessCalculator;
            _initializer = initializer;
            _measureMutationOperators = measureMutationOperators;
            _melodyMutationOperators = melodyMutationOperators;
            _replacementOperator = replacementOperator;
            _selector = selector;
            _stopChecker = stopChecker;
            Generations = new List<Population>();
        }

        public ICollection<Population> Generations { get; set; }
        private Melody BestEver => Generations.SelectMany(g => g.Individuals).OrderByDescending(i => i.Fitness).First();

        public Melody Solve()
        {
            var currentPopulation = _initializer.Initialize();
            ApplyMutationOperators(currentPopulation);
            currentPopulation.Individuals.ToList().ForEach(currentMelody => _fitnessCalculator.Calculate(_initializer.BaseMelody, currentMelody));
            Generations.Add(currentPopulation);

            while (!_stopChecker.Stop(this))
            {
                var newPopulation = _replacementOperator.Replace(currentPopulation);
                newPopulation.Individuals.ToList().ForEach(currentMelody => _fitnessCalculator.Calculate(_initializer.BaseMelody, currentMelody));
                var best = newPopulation.BestIndividual();
                Print(best, newPopulation.Number.ToString());
                PrintFitnessValues(best);

                Generations.Add(newPopulation);
                currentPopulation = newPopulation;
            }

            Print(_initializer.BaseMelody, "input");
            PrintFitnessValues(_initializer.BaseMelody);

            Print(BestEver, "output");
            PrintFitnessValues(BestEver);

            return BestEver;
        }

        private void ApplyMutationOperators(Population population)
        {
            foreach (var melody in population.Individuals)
            {
                _melodyMutationOperators.ForEach(o => o.Mutate(melody));

                foreach (var measure in melody.Measures)
                {
                    _measureMutationOperators.ForEach(o => o.Mutate(measure));
                }
            }
        }

        private double Divide(double a, double b)
        {
            if (b == 0)
            {
                return 0;
            }

            return a / b;
        }

        private List<int> GetDifferentIntervals(Melody melody)
        {
            var differentInvervals = new List<int>();
            var notes = melody.Measures.SelectMany(m => m.Events).OfType<Note>().ToList();

            Note previous = null;
            foreach (var note in notes)
            {
                if (previous != null)
                {
                    var interval = Math.Abs(previous.Number - note.Number);

                    if (interval > 0 && !differentInvervals.Contains(interval))
                    {
                        differentInvervals.Add(interval);
                    }
                }

                previous = note;
            }

            return differentInvervals;
        }

        private void Print(Melody melody, string id)
        {
            Console.WriteLine($" {id}");
            Console.SetWindowSize(Console.LargestWindowWidth - 20, Console.LargestWindowHeight);
            Console.Write($" Fitness: {melody.Fitness}\n\n");
            foreach (var measure in melody.Measures)
            {
                Console.Write($" Compasso {measure.Order.ToString("00")}: ");
                measure.Events.ToList().ForEach(e => Console.Write($"{PrintEvent(e.Number)} "));
                Console.Write("\n\n");
            }
        }

        private string PrintEvent(int number)
        {
            if (number == (int)RestOrTie.Rest)
            {
                return "PAUS";
            }

            if (number == (int)RestOrTie.Tie)
            {
                return "SUST";
            }

            return NoteUtilities.GetNoteName((SevenBitNumber)number).ToString();
        }

        private void PrintFitnessValues(Melody melody)
        {
            double originalNoteDensity = Divide(melody.Measures.SelectMany(m => m.Events).OfType<Note>().Count(), melody.Measures.SelectMany(m => m.Events).Count());
            double originalPichVariety = Divide(melody.Measures.SelectMany(m => m.Events).OfType<Note>().GroupBy(note => note.Number).Count(), melody.Measures.SelectMany(m => m.Events).OfType<Note>().Count());
            double originalRestDensity = Divide(melody.Measures.SelectMany(m => m.Events).OfType<Rest>().Count(), melody.Measures.SelectMany(m => m.Events).Count());
            double originalTieDensity = Divide(melody.Measures.SelectMany(m => m.Events).OfType<Tie>().Count(), melody.Measures.SelectMany(m => m.Events).Count());
            var originalDifferentIntervals = GetDifferentIntervals(melody);

            Console.WriteLine($" Densidade de notas:    {originalNoteDensity}");
            Console.WriteLine($" Variedade de notas:    {originalPichVariety}");
            Console.WriteLine($" Densidade de pausas:   {originalRestDensity}");
            Console.WriteLine($" Densidade de sust.:    {originalTieDensity}");
            Console.WriteLine($" Intervalos diferentes: {originalDifferentIntervals.Count}");

            Console.WriteLine("---------------------------------------------------------");
        }
    }
}