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
            // Validate input (no negative divisors, etc.)
            if (request.Rules.Any(r => r.Divisor <= 0))
                return BadRequest("Divisor must be a positive integer.");

            // Create Game entity
            var game = new Games
            {
                Name = request.Name,
                Author = request.Author,
                CreatedAt = DateTime.UtcNow
            };

            // Add associated rules
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
