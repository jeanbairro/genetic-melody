namespace GeneticMelody.Genetic
{
    public class Event
    {
        public Event()
        {
        }

        public Event(int number, int order)
        {
            Number = number;
            Order = order;
        }

        public int Number { get; set; }
        public int Order { get; set; }
    }
}