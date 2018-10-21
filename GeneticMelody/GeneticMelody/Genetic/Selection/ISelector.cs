using GeneticMelody.Genetic.Domain;
using System;
using System.Collections.Generic;

namespace GeneticMelody.Genetic.Selection
{
    public interface ISelector
    {
        ICollection<Melody> Select(Population population);
    }
}