namespace FizzBuzz.Models
{
    public class Games
    {

        public int Id { get; set; } 
        public string Name { get; set; } = default!;

        public string Author { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public ICollection<GameRule> Rules { get; set; } = new List<GameRule>();

        public int MinNumber { get; set; } = 1;

        public int MaxNumber { get; set; } = 100;
    }
}
