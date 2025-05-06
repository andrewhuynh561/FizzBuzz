using Microsoft.AspNetCore.Mvc;
using FizzBuzz.Services;
using FizzBuzz.Models;
using FizzBuzz.Contracts;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FizzBuzz.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// POST /api/games
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] CreateGame request)
        {
            var result = await _gameService.CreateGameAsync(request);
            return StatusCode(result.StatusCode, result.Success ? (object)new { gameId = result.Data.Id } : result.ErrorMessage);
        }

        /// <summary>
        /// DELETE /api/Games/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var result = await _gameService.DeleteGameAsync(id);
            return StatusCode(result.StatusCode, result.Success ? null : result.ErrorMessage);
        }

        /// <summary>
        /// GET /api/games/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            var result = await _gameService.GetGameByIdAsync(id);
            return StatusCode(result.StatusCode, result.Success ? (object)result.Data : result.ErrorMessage);
        }

        /// <summary>
        /// GET /api/games
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var result = await _gameService.GetAllGamesAsync();
            return StatusCode(result.StatusCode, result.Success ? (object)result.Data : result.ErrorMessage);
        }
    }
}
