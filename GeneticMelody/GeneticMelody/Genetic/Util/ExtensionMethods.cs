using Melanchall.DryWetMidi.Smf.Interaction;
using System.Collections.Generic;
using System.Linq;

namespace GeneticMelody.Genetic.Util
{
    public static class ExtensionMethods
    {
        public static ICollection<T> Swap<T>(this System.Collections.Generic.IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        public static ICollection<T> Shuffle<T>(this System.Collections.Generic.IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        public static IEnumerable<ILengthedObject> PlayingAtTime(this IEnumerable<ILengthedObject> list, long time)
        {
            return list.Where(e => time >= e.Time && time < (e.Time + e.Length));
        }

        public static bool Equal(this ILengthedObject obj, ILengthedObject objB)
        {
            return obj.Time == objB?.Time && obj.Length == objB?.Length && obj.GetType() == objB?.GetType();
        }
    }
}