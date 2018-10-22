using GeneticMelody.Converter;
using GeneticMelody.Genetic.Crossover;
using GeneticMelody.Genetic.FitnessCalculators;
using GeneticMelody.Genetic.Initialization;
using GeneticMelody.Genetic.Replacement;
using GeneticMelody.Genetic.Selection;
using GeneticMelody.Genetic.StoppingCriterion;
using GeneticMelody.Util;
using Melanchall.DryWetMidi.Smf;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.Domain
{
    public class Solver
    {
        private readonly ICrossoverOperator _crossoverOperator;
        private readonly ISimilarityFitnessCalculator _fitnessCalculator;
        private readonly IInitializazer _initializer;
        private readonly IReplacementOperator _replacementOperator;
        private readonly ISelector _selector;
        private readonly IStoppingCriterionChecker _stopChecker;

        public Solver(ICrossoverOperator crossoverOperator, ISimilarityFitnessCalculator fitnessCalculator, IInitializazer initializer, IReplacementOperator replacementOperator, ISelector selector, IStoppingCriterionChecker stopChecker)
        {
            _crossoverOperator = crossoverOperator;
            _fitnessCalculator = fitnessCalculator;
            _initializer = initializer;
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
            /*aplicar operadores de mutação*/
            currentPopulation.Individuals.ToList().ForEach(currentMelody => _fitnessCalculator.Calculate(_initializer.BaseMelody, currentMelody));
            Generations.Add(currentPopulation);

            while (!_stopChecker.Stop(this))
            {
                var newPopulation = _replacementOperator.Replace(currentPopulation);
                newPopulation.Individuals.ToList().ForEach(currentMelody => _fitnessCalculator.Calculate(_initializer.BaseMelody, currentMelody));
                Generations.Add(newPopulation);
                currentPopulation = newPopulation;
            }

            return BestEver;
        }
    }
}