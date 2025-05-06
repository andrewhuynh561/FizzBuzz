using Microsoft.AspNetCore.Mvc;
using FizzBuzz.Models;
using FizzBuzz.Services;
using System.Threading.Tasks;

namespace FizzBuzz.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        /// <summary>
        /// POST /api/sessions/start
        /// </summary>
        [HttpPost("start")]
        public async Task<IActionResult> StartSession([FromBody] StartSession request)
        {
            var result = await _sessionService.StartSessionAsync(request);
            return StatusCode(result.StatusCode, result.Success ? 
                (object)new { sessionId = result.Data?.Id, endTime = result.Data?.EndTime } : 
                result.ErrorMessage);
        }

        /// <summary>
        /// GET /api/sessions/{sessionId}/next-number
        /// </summary>
        [HttpGet("{sessionId}/next-number")]
        public async Task<IActionResult> GetNextNumber(int sessionId)
        {
            var result = await _sessionService.GetNextNumberAsync(sessionId);
            return StatusCode(result.StatusCode, result.Success ? 
                (object)new { number = result.Data.Number } : 
                result.ErrorMessage);
        }

        /// <summary>
        /// POST /api/sessions/{sessionId}/answer
        /// </summary>
        [HttpPost("{sessionId}/answer")]
        public async Task<IActionResult> SubmitAnswer(int sessionId, [FromBody] SubmitAnswer request)
        {
            var result = await _sessionService.SubmitAnswerAsync(sessionId, request);
            return StatusCode(result.StatusCode, result.Success ? 
                (object)new { correct = result.Data.IsCorrect, expected = result.Data.ExpectedAnswer } : 
                result.ErrorMessage);
        }

        /// <summary>
        /// GET /api/sessions/{sessionId}/results
        /// </summary>
        [HttpGet("{sessionId}/results")]
        public async Task<IActionResult> GetResults(int sessionId)
        {
            var result = await _sessionService.GetSessionResultsAsync(sessionId);
            return StatusCode(result.StatusCode, result.Success ? 
                (object)new { correctCount = result.Data.CorrectCount, incorrectCount = result.Data.IncorrectCount } : 
                result.ErrorMessage);
        }
    }
}
