using System.ComponentModel.DataAnnotations;

namespace FizzBuzz.Models
{
    public class SubmitAnswer
    {
        [Required]
        public int Number { get; set; }
        
        [Required]
        public string UserAnswer { get; set; }
    }
}
