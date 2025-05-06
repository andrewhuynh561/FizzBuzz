using System.ComponentModel.DataAnnotations;

namespace FizzBuzz.Models
{
    public class StartSession
    {
        [Required]
        public int GameId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be a positive number")]
        public int DurationSeconds { get; set; }
    }
}
