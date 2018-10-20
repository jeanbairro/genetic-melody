using GeneticMelody.Genetic.Domain;

namespace GeneticMelody.Genetic.Initialization
{
    public interface IInitializazer
    {
        Population Initialize();
    }
}