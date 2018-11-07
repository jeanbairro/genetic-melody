using System;

namespace GeneticMelody.Genetic.Domain
{
    public class TypeEventRate
    {
        public TypeEventRate(Type type, double rate)
        {
            Type = type;
            Rate = rate;
        }

        public double Rate { get; set; }
        public Type Type { get; set; }
    }
}