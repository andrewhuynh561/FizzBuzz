namespace FizzBuzz.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Games? Game { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int CorrectCount { get; set; }
        public int IncorrectCount { get; set; }
    }
}
