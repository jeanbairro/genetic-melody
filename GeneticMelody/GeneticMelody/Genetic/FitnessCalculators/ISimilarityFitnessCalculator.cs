namespace GeneticMelody.Genetic.FitnessCalculators
{
    public interface ISimilarityFitnessCalculator
    {
        void Calculate(Melody originalMelody, Melody currentMelody);
    }
}