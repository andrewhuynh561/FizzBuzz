namespace FizzBuzz.Contracts
{
    public class CreateGameRuleRequest
    {
        public int Divisor { get; set; }
        public string ReplacementText { get; set; } = default!;
    }
}
