namespace GeneticMelody.Genetic.FitnessCalculators
{
    internal interface ISimilarityFitnessCalculator
    {
        double Calculate(Melody originalMelody, Melody currentMelody);
    }
}