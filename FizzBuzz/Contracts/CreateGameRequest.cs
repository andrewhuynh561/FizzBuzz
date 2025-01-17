namespace FizzBuzz.Contracts
{
    public class CreateGameRequest
    {
        public string Name { get; set; } = default!;
        public string Author { get; set; } = default!;
        public List<CreateGameRuleRequest> Rules { get; set; } = new();
    }
}
