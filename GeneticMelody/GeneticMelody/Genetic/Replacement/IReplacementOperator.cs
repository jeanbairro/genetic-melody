using GeneticMelody.Genetic.Domain;

namespace GeneticMelody.Genetic.Replacement
{
    public interface IReplacementOperator
    {
        Population Replace(Population population);
    }
}