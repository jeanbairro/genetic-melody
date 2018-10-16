namespace GeneticMelody.Genetic.FitnessCalculators
{
    interface ISimilarityFitnessCalculator
    {
        double Calculate(Melody originalMelody, Melody currentMelody);
    }
}
