using GeneticMelody.Genetic.Domain;

namespace GeneticMelody.Genetic.StoppingCriterion
{
    public interface IStoppingCriterionChecker
    {
        bool Stop(Solver solver);
    }
}