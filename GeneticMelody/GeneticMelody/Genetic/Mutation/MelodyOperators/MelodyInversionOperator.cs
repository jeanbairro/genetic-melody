using GeneticMelody.Genetic.Util;
using System.Linq;

namespace GeneticMelody.Genetic.Mutation.MelodyOperators
{
    public class MelodyInversionOperator : IMelodyMutationOperator, IMutationOperator
    {
        public MelodyInversionOperator(int rate)
        {
            Rate = rate;
        }

        public int Rate { get; set; }

        public void Mutate(Melody melody)
        {
            var randomRate = ThreadSafeRandom.ThisThreadsRandom.Next(0, 100);

            if (randomRate < Rate)
            {
                var measures = melody.Measures.OrderByDescending(m => m.Order).ToList();
                for (var i = 0; i < measures.Count; i++)
                {
                    measures[i].Order = i;
                }

                melody.Measures = measures.ToList();
            }
        }
    }
}