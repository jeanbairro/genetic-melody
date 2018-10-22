namespace GeneticMelody.Genetic.Mutation
{
    public interface IMeasureMutationOperator
    {
        Measure Mutate(Measure melody);
    }
}