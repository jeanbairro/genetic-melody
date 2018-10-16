using GeneticMelody.Genetic.Domain.Interfaces;

namespace GeneticMelody.Genetic.GeneticOperators.Initializer
{
    public interface IInitializer
    {
        IPopulation Initialize();
    }
}
