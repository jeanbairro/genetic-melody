namespace GeneticMelody.Genetic
{
    public class Note : Event
    {
        public Note(string name, int number) : base(number)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
