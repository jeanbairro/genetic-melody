using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.FitnessCalculators
{
    public class MelodySimilarityFitnessCalculator : ISimilarityFitnessCalculator
    {
        public void Calculate(Melody originalMelody, Melody currentMelody)
        {
            double originalNoteDensity = Divide(originalMelody.Measures.SelectMany(m => m.Events).OfType<Note>().Count(), originalMelody.Measures.SelectMany(m => m.Events).Count());
            double currentNoteDensity = Divide(currentMelody.Measures.SelectMany(m => m.Events).OfType<Note>().Count(), currentMelody.Measures.SelectMany(m => m.Events).Count());
            double originalPichVariety = Divide(originalMelody.Measures.SelectMany(m => m.Events).OfType<Note>().GroupBy(note => note.Number).Count(), originalMelody.Measures.SelectMany(m => m.Events).OfType<Note>().Count());
            double currentPichVariety = Divide(currentMelody.Measures.SelectMany(m => m.Events).OfType<Note>().GroupBy(note => note.Number).Count(), currentMelody.Measures.SelectMany(m => m.Events).OfType<Note>().Count());
            double originalRestDensity = Divide(originalMelody.Measures.SelectMany(m => m.Events).OfType<Rest>().Count(), originalMelody.Measures.SelectMany(m => m.Events).Count());
            double currentRestDensity = Divide(currentMelody.Measures.SelectMany(m => m.Events).OfType<Rest>().Count(), currentMelody.Measures.SelectMany(m => m.Events).Count());
            double originalTieDensity = Divide(originalMelody.Measures.SelectMany(m => m.Events).OfType<Tie>().Count(), originalMelody.Measures.SelectMany(m => m.Events).Count());
            double currentTieDensity = Divide(currentMelody.Measures.SelectMany(m => m.Events).OfType<Tie>().Count(), currentMelody.Measures.SelectMany(m => m.Events).Count());
            var originalDifferentIntervals = GetDifferentIntervals(originalMelody);
            var currentDifferentIntervals = GetDifferentIntervals(currentMelody);

            var pitchVarietySimilarity = CalculateDifference(originalPichVariety, currentPichVariety);
            var noteDensitySimilarity = CalculateDifference(originalNoteDensity, currentNoteDensity);
            var restDensitySimilarity = CalculateDifference(originalRestDensity, currentRestDensity);
            var tieDensitySimilarity = CalculateDifference(originalTieDensity, currentTieDensity);
            var diffIntervalsSimilarity = CalculateDifference(originalDifferentIntervals, currentDifferentIntervals);

            currentMelody.Fitness =
                (pitchVarietySimilarity * 1) +
                (noteDensitySimilarity * 1) +
                (restDensitySimilarity * 1) +
                (tieDensitySimilarity * 1) +
                (diffIntervalsSimilarity * 1);
        }

        private double Divide(double a, double b)
        {
            if (b == 0)
            {
                return 0;
            }

            return a / b;
        }

        // 1 − |a−b| / L. Where L is the maximum value that any of the two numbers can take.
        private double CalculateDifference(double originalValue, double approximateValue) => 1 - Math.Abs(originalValue - approximateValue) / 1;

        private double CalculateDifference(List<int> originalMelodyIntervals, List<int> currentMelodyIntervals)
        {
            var intervalsNotPresentInOriginalMelody = currentMelodyIntervals.Where(i => !originalMelodyIntervals.Contains(i)).ToList();
            var division = intervalsNotPresentInOriginalMelody.Count / (double)currentMelodyIntervals.Count;
            return 1 - (double.IsNaN(division) ? 0 : division);
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