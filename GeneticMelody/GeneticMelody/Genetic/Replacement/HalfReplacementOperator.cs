using GeneticMelody.Genetic.Crossover;
using GeneticMelody.Genetic.Domain;
using GeneticMelody.Genetic.Selection;
using System.Linq;

namespace GeneticMelody.Genetic.Replacement
{
    public class HalfReplacementOperator : IReplacementOperator
    {
        private readonly ICrossoverOperator _crossoverOperator;
        private readonly ISelector _selector;

        public HalfReplacementOperator(ICrossoverOperator crossoverOperator, ISelector selector)
        {
            _crossoverOperator = crossoverOperator;
            _selector = selector;
        }

        public Population Replace(Population population)
        {
            var newPopulation = new Population();

            while (newPopulation.Individuals.Count < Population.Limit)
            {
                // Get individuals selected ordered desc by fitness
                var selected = _selector.Select(population);

                var firstParent = selected.ElementAt(0);
                var secondParent = selected.ElementAt(1);

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