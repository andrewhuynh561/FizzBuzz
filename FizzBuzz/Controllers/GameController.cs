using Microsoft.AspNetCore.Mvc;
using FizzBuzz.Data;
using FizzBuzz.Models;
using FizzBuzz.Contracts;
using Microsoft.EntityFrameworkCore;


namespace FizzBuzz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly FizzBuzzDbContext _db;

        public GamesController(FizzBuzzDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// POST /api/games
        /// Creates a new custom FizzBuzz-like game definition
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest request)
        {
            // checking input 
            if (request.Rules.Any(r => r.Divisor <= 0))
                return BadRequest("Divisor must be a positive integer.");

            var game = new Games
            {
                Name = request.Name,
                Author = request.Author,
                CreatedAt = DateTime.UtcNow
            };

            // Add  rules
            foreach (var ruleReq in request.Rules)
            {
                game.Rules.Add(new GameRule
                {
                    Divisor = ruleReq.Divisor,
                    ReplacementText = ruleReq.ReplacementText
                });
            }

            _db.Games.Add(game);
            await _db.SaveChangesAsync();

            return Ok(new { gameId = game.Id });
        }
        // DELETE /api/Games/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _db.Games.FindAsync(id);
            if (game == null)
                return NotFound();

            // Also remove any dependent data (like rules) if you have
            // cascade deletes or manual remove logic
            _db.Games.Remove(game);
            await _db.SaveChangesAsync();

            return NoContent(); // or Ok()
        }

        /// <summary>
        /// GET /api/games/{id}
        /// Retrieves a specific game definition by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            var game = await _db.Games
                                .Include(g => g.Rules)
                                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
                return NotFound();

            return Ok(game);
        }

        /// <summary>
        /// GET /api/games
        /// Retrieve all game definitions (example usage).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _db.Games
                                 .Include(g => g.Rules)
                                 .ToListAsync();

            return Ok(games);
        }
    }
}
