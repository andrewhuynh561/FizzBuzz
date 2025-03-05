namespace FizzBuzz.Contracts
{
    public class CreateGame
    {
        public string Name { get; set; } = default!;
        public string Author { get; set; } = default!;
        public List<CreateRule> Rules { get; set; } = new();

        public int MinNumber { get; set; } = 1;

        public int MaxNumber { get; set; } = 100; 


    }
}
