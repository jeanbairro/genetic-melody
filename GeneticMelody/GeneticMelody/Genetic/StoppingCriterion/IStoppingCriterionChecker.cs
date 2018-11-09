using GeneticMelody.Genetic.Domain;

namespace GeneticMelody.Genetic.StoppingCriterion
{
    public interface IStoppingCriterionChecker
    {
        bool Stop(MelodyGenerator solver, GeneticConfiguration configuration);
    }
}