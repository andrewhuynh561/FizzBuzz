using Microsoft.AspNetCore.Mvc;
using FizzBuzz.Data;
using FizzBuzz.Models;
using FizzBuzz.Contracts;
using Microsoft.EntityFrameworkCore;
using FizzBuzz.Data;
using FizzBuzz.Models;

namespace FizzBuzz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly FizzBuzzDbContext _db;

        public SessionsController(FizzBuzzDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// POST /api/sessions/start
        /// Starts a new game session for the specified game
        /// </summary>
        [HttpPost("start")]
        public async Task<IActionResult> StartSession([FromBody] StartSession request)
        {
            // Validate the game exists
            var game = await _db.Games.Include(g => g.Rules)
                                      .FirstOrDefaultAsync(g => g.Id == request.GameId);

            if (game == null)
                return NotFound("Game not found.");

            // Create new session
            var session = new Session
            {
                GameId = game.Id,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddSeconds(request.DurationSeconds),
                CorrectCount = 0,
                IncorrectCount = 0
            };

            _db.Sessions.Add(session);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                sessionId = session.Id,
                endTime = session.EndTime
            });
        }

        /// <summary>
        /// GET /api/sessions/{sessionId}/next-number
        /// Retrieves a new random non-duplicate number for the session
        /// </summary>
        [HttpGet("{sessionId}/next-number")]
        public async Task<IActionResult> GetNextNumber(int sessionId)
        {
            var session = await _db.Sessions
                .Include(s => s.Game)
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
                return NotFound("Session not found.");

            if (DateTime.UtcNow > session.EndTime)
                return BadRequest("Session has expired.");

            // Use the range from the game
            var min = session.Game!.MinNumber;
            var max = session.Game!.MaxNumber;

            var random = new Random();
            int nextNum;
            bool isDuplicate;
            do
            {
                // random.Next(min, max+1) so it includes maxNumber
                nextNum = random.Next(min, max + 1);
                isDuplicate = await _db.SessionRandomNumbers
                    .AnyAsync(x => x.SessionId == sessionId && x.Number == nextNum);
            } while (isDuplicate);

            _db.SessionRandomNumbers.Add(new SessionRandomNumber
            {
                SessionId = sessionId,
                Number = nextNum
            });
            await _db.SaveChangesAsync();

            return Ok(new { number = nextNum });
        }


        /// <summary>
        /// POST /api/sessions/{sessionId}/answer
        /// Submits an answer for the given random number
        /// </summary>
        [HttpPost("{sessionId}/answer")]
        public async Task<IActionResult> SubmitAnswer(int sessionId, [FromBody] SubmitAnswer request)
        {
            var session = await _db.Sessions
                                   .Include(s => s.Game)
                                   .ThenInclude(g => g.Rules)
                                   .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
                return NotFound("Session not found.");

            // Check if session is expired
            if (DateTime.UtcNow > session.EndTime)
                return BadRequest("Session has expired.");

            var computedAnswer = ComputeReplacement(request.Number, session.Game!.Rules);

            // Compare answer
            bool isCorrect = string.Equals(request.UserAnswer, computedAnswer, StringComparison.OrdinalIgnoreCase);

            if (isCorrect)
                session.CorrectCount++;
            else
                session.IncorrectCount++;

            await _db.SaveChangesAsync();

            return Ok(new
            {
                correct = isCorrect,
                expected = computedAnswer
            });
        }

        /// <summary>
        /// GET /api/sessions/{sessionId}/results
        /// Gets the final scoreboard for a session (correct/incorrect counts)
        /// </summary>
        [HttpGet("{sessionId}/results")]
        public async Task<IActionResult> GetResults(int sessionId)
        {
            var session = await _db.Sessions.FindAsync(sessionId);
            if (session == null)
                return NotFound("Session not found.");

            return Ok(new
            {
                correctCount = session.CorrectCount,
                incorrectCount = session.IncorrectCount
            });
        }

        /// <summary>
        /// FizzBuzz-like logic to compute the replacement string for a number
        /// </summary>
        private string ComputeReplacement(int number, ICollection<GameRule> rules)
        {
            var result = string.Empty;

            foreach (var rule in rules)
            {
                if (number % rule.Divisor == 0)
                {
                    result += rule.ReplacementText;
                }
            }

            // If no divisors matched, just return the number
            if (string.IsNullOrEmpty(result))
                result = number.ToString();

            return result;
        }
    }
}
