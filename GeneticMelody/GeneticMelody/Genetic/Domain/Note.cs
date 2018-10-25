namespace GeneticMelody.Genetic
{
    public class Note : Event
    {
        public Note(string name, int number, int order) : base(number, order)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}