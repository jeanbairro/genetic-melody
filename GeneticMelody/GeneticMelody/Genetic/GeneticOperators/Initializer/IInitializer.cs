using GeneticMelody.Genetic.Domain;

namespace GeneticMelody.Genetic.GeneticOperators.Initializer
{
    public interface IInitializer
    {
        Population Initialize();
    }
}