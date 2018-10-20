using System.Collections.Generic;

namespace GeneticMelody.Genetic.Crossover
{
    public interface ICrossoverOperator
    {
        Melody Cross(Melody firstParent, Melody secondParent);
    }
}