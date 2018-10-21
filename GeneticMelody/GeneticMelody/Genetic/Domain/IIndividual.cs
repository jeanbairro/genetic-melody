using System;

namespace GeneticMelody.Genetic.Domain
{
    public interface IIndividual
    {
        double Fitness { get; set; }
        Guid Identity { get; set; }
    }
}