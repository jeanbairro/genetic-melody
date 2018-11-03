using System.Linq;

namespace GeneticMelody.Genetic.Util
{
    public sealed class Singleton<T> where T : class, new()
    {
        private static T instance;

        public static T Instance()
        {
            lock (typeof(T))
                if (instance == null) instance = new T();

            return instance;
        }
    }
}