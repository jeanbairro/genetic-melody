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
            var newPopulation = (Population)population.Clone();

            while (newPopulation.Individuals.Count < Population.Limit)
            {
                // Get individuals selected ordered desc by fitness
                var selected = _selector.Select(population);

                // Take the parents
                var firstParent = selected.ElementAt(0);
                var secondParent = selected.ElementAt(1);

                // Take the individuals for replace
                var firstRemoved = newPopulation.Individuals.First(i => i.Identity == selected.ElementAt(2).Identity);
                var secondRemoved = newPopulation.Individuals.First(i => i.Identity == selected.ElementAt(3).Identity);

                var firstChild = _crossoverOperator.Cross(firstParent, secondParent);
                var secondChild = _crossoverOperator.Cross(secondParent, firstParent);

                newPopulation.Individuals.Remove(firstRemoved);
                newPopulation.Individuals.Remove(secondRemoved);
                newPopulation.Individuals.Add(firstChild);
                newPopulation.Individuals.Add(secondChild);
            }

            return newPopulation;
        }
    }
}