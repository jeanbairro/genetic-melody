using GeneticMelody.Genetic.Domain;

namespace GeneticMelody.Genetic.Mutation.MelodyOperators
{
    public interface IMelodyReplacementOperator
    {
        void Mutate(Melody melody, Population population);
    }
}