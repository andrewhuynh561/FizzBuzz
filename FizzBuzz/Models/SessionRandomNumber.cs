namespace FizzBuzz.Models
{
    public class SessionRandomNumber
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public Session? Session { get; set; }

        public int Number { get; set; }
    }
}
