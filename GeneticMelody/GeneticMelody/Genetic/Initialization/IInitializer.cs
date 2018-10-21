using GeneticMelody.Genetic.Domain;

namespace GeneticMelody.Genetic.Initialization
{
    public interface IInitializazer
    {
        Melody BaseMelody { get; set; }

        Population Initialize();
    }
}