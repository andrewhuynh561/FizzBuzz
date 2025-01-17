namespace FizzBuzz.Models

{
    public class GameRule
    {
        public int Id { get; set; } 

        public int GameId { get; set; } 

        public Games? Game { get; set; } 

        public int Divisor  { get; set; }

        public string ReplacementText { get; set; } = default!;
    }
}
