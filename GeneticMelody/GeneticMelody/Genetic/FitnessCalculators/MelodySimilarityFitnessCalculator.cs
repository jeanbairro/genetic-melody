using System;

namespace GeneticMelody.Genetic.FitnessCalculators
{
    public class MelodySimilarityFitnessCalculator : ISimilarityFitnessCalculator
    {
        public double Calculate(Melody originalMelody, Melody currentMelody)
        {
            var pitchVarietySimilarity = 1 - Math.Abs(originalMelody.PichVariety - currentMelody.PichVariety) / originalMelody.PichVariety;
            var noteDensitySimilarity = 1 - Math.Abs(originalMelody.NoteDensity - currentMelody.NoteDensity) / originalMelody.NoteDensity;
            var restDensitySimilarity = 1 - Math.Abs(originalMelody.RestDensity - currentMelody.RestDensity) / originalMelody.RestDensity;
            var diffIntervalsSimilarity = 1 - Math.Abs(originalMelody.DifferentIntervals - currentMelody.DifferentIntervals) / originalMelody.DifferentIntervals;

            return pitchVarietySimilarity + noteDensitySimilarity + restDensitySimilarity + diffIntervalsSimilarity;
        }
    }
}