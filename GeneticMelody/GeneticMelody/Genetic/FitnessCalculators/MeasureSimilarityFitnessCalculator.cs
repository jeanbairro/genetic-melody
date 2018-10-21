using System;
using System.Linq;

namespace GeneticMelody.Genetic.FitnessCalculators
{
    public class MeasureSimilarityFitnessCalculator : ISimilarityFitnessCalculator
    {
        public void Calculate(Melody originalMelody, Melody currentMelody)
        {
            var pitchVarietySimilarity = 1 - Math.Abs(originalMelody.Measures.Sum(m => m.PichVariety) - currentMelody.Measures.Sum(m => m.PichVariety)) / originalMelody.Measures.Sum(m => m.PichVariety);
            var noteDensitySimilarity = 1 - Math.Abs(originalMelody.Measures.Sum(m => m.NoteDensity) - currentMelody.Measures.Sum(m => m.NoteDensity)) / originalMelody.Measures.Sum(m => m.NoteDensity);
            var restDensitySimilarity = 1 - Math.Abs(originalMelody.Measures.Sum(m => m.RestDensity) - currentMelody.Measures.Sum(m => m.RestDensity)) / originalMelody.Measures.Sum(m => m.RestDensity);
            var diffIntervalsSimilarity = 1 - Math.Abs(originalMelody.Measures.Sum(m => m.DifferentIntervals) - currentMelody.Measures.Sum(m => m.DifferentIntervals)) / originalMelody.Measures.Sum(m => m.DifferentIntervals);

            currentMelody.Fitness = pitchVarietySimilarity + noteDensitySimilarity + restDensitySimilarity + diffIntervalsSimilarity;
        }
    }
}