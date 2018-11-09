using GeneticMelody.Genetic.Crossover;
using GeneticMelody.Genetic.Domain;
using GeneticMelody.Genetic.Mutation.MeasureOperators;
using GeneticMelody.Genetic.Selection;
using System.Collections;
using System.Linq;

namespace GeneticMelody.Genetic.Replacement
{
    public class HalfReplacementOperator : IReplacementOperator
    {
        private readonly GeneticConfiguration _configuration;
        private readonly ICrossoverOperator _crossoverOperator;
        private readonly ISelector _selector;

        public HalfReplacementOperator(ICrossoverOperator crossoverOperator, ISelector selector, GeneticConfiguration configuration)
        {
            _crossoverOperator = crossoverOperator;
            _selector = selector;
            _configuration = configuration;
        }

        public Population Replace(Population population)
        {
            var newPopulation = new Population(population.Sequence + 1, _configuration);

            while (newPopulation.Individuals.Count < newPopulation.Limit)
            {
                // Get individuals selected ordered desc by fitness
                var selected = _selector.Select(population);

                var firstParent = selected.ElementAt(0).Clone();
                var secondParent = selected.ElementAt(1).Clone();

                var firstChild = _crossoverOperator.Cross(firstParent, secondParent);
                var secondChild = _crossoverOperator.Cross(secondParent, firstParent);

                newPopulation.Individuals.Add(firstParent);
                newPopulation.Individuals.Add(secondParent);
                newPopulation.Individuals.Add(firstChild);
                newPopulation.Individuals.Add(secondChild);
            }

            return newPopulation;
        }
    }
}