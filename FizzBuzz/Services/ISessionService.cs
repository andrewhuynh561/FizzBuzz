using FizzBuzz.Contracts;
using FizzBuzz.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FizzBuzz.Services
{
    public interface ISessionService
    {
        Task<Models.ServiceResult<Session>> StartSessionAsync(Models.StartSession request);
        Task<Models.ServiceResult<SessionNumberResult>> GetNextNumberAsync(int sessionId);
        Task<Models.ServiceResult<AnswerResult>> SubmitAnswerAsync(int sessionId, Models.SubmitAnswer request);
        Task<Models.ServiceResult<SessionResult>> GetSessionResultsAsync(int sessionId);
    }

    public class SessionNumberResult
    {
        public int Number { get; set; }
    }

    public class AnswerResult
    {
        public bool IsCorrect { get; set; }
        public string ExpectedAnswer { get; set; }
    }

    public class SessionResult
    {
        public int CorrectCount { get; set; }
        public int IncorrectCount { get; set; }
    }
}
