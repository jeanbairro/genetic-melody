using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.FitnessCalculators
{
    public class MelodySimilarityFitnessCalculator : ISimilarityFitnessCalculator
    {
        public void Calculate(Melody originalMelody, Melody currentMelody)
        {
            double originalNoteDensity = originalMelody.Measures.SelectMany(m => m.Events).OfType<Note>().Count() / (double)originalMelody.Measures.SelectMany(m => m.Events).Count();
            double currentNoteDensity = currentMelody.Measures.SelectMany(m => m.Events).OfType<Note>().Count() / (double)currentMelody.Measures.SelectMany(m => m.Events).Count();
            double originalPichVariety = originalMelody.Measures.SelectMany(m => m.Events).OfType<Note>().GroupBy(note => note.Number).Count() / (double)originalMelody.Measures.SelectMany(m => m.Events).OfType<Note>().Count();
            double currentPichVariety = currentMelody.Measures.SelectMany(m => m.Events).OfType<Note>().GroupBy(note => note.Number).Count() / (double)currentMelody.Measures.SelectMany(m => m.Events).OfType<Note>().Count();
            double originalRestDensity = originalMelody.Measures.SelectMany(m => m.Events).OfType<Rest>().Count() / (double)originalMelody.Measures.SelectMany(m => m.Events).Count();
            double currentRestDensity = currentMelody.Measures.SelectMany(m => m.Events).OfType<Rest>().Count() / (double)currentMelody.Measures.SelectMany(m => m.Events).Count();
            var originalDifferentIntervals = GetDifferentIntervals(originalMelody);
            var currentDifferentIntervals = GetDifferentIntervals(currentMelody);

            var pitchVarietySimilarity = CalculateDifference(originalPichVariety, currentPichVariety);
            var noteDensitySimilarity = CalculateDifference(originalNoteDensity, currentNoteDensity);
            var restDensitySimilarity = CalculateDifference(originalRestDensity, currentNoteDensity);
            var diffIntervalsSimilarity = CalculateDifference(originalDifferentIntervals, currentDifferentIntervals);

            currentMelody.Fitness =
                pitchVarietySimilarity +
                noteDensitySimilarity +
                restDensitySimilarity +
                diffIntervalsSimilarity;
        }

        // 1 − |a−b| / L. Where L is the maximum value that any of the two numbers can take.
        private double CalculateDifference(double originalValue, double approximateValue) => 1 - Math.Abs(originalValue - approximateValue) / 1;

        private double CalculateDifference(List<int> originalMelodyIntervals, List<int> currentMelodyIntervals)
        {
            var intervalsNotPresentInOriginalMelody = currentMelodyIntervals.Where(i => !originalMelodyIntervals.Contains(i)).ToList();
            return 1 - intervalsNotPresentInOriginalMelody.Count / (double)currentMelodyIntervals.Count;
        }

        private List<int> GetDifferentIntervals(Melody melody)
        {
            var differentInvervals = new List<int>();
            var notes = melody.Measures.SelectMany(m => m.Events).OfType<Note>().ToList();

            Note previous = null;
            foreach (var note in notes)
            {
                if (previous != null)
                {
                    var interval = Math.Abs(previous.Number - note.Number);

                    if (interval > 0 && !differentInvervals.Contains(interval))
                    {
                        differentInvervals.Add(interval);
                    }
                }

                previous = note;
            }

            return differentInvervals;
        }
    }
}