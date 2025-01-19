namespace FizzBuzz.Contracts
{
    public class SubmitAnswerRequest
    {
        public int Number { get; set; }
        public string UserAnswer { get; set; } = default!;
    }
}
