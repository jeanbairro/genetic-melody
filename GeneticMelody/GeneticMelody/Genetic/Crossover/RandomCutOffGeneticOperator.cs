using System;
using System.Linq;

namespace GeneticMelody.Genetic.Crossover
{
    public class RandomCutOffGeneticOperator : ICrossoverOperator
    {
        public Melody Cross(Melody firstParent, Melody secondParent)
        {
            var measuresCount = firstParent.Measures.Count;
            var randomizer = new Random();
            var numberOfFirstTake = randomizer.Next(measuresCount + 1);
            var firstMeasures = firstParent.Measures.Take(numberOfFirstTake).ToList();
            var secondMeasures = secondParent.Measures.Skip(numberOfFirstTake).Take(measuresCount - numberOfFirstTake).ToList();
            firstMeasures.AddRange(secondMeasures);

            return new Melody(firstMeasures, firstParent.TempoMap);
        }
    }
}