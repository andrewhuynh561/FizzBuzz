using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzBuzz.Data;
using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzz.Services
{
    public class SessionService : ISessionService
    {
        private readonly FizzBuzzDbContext _db;

        public SessionService(FizzBuzzDbContext db)
        {
            _db = db;
        }

        public async Task<Models.ServiceResult<Session>> StartSessionAsync(Models.StartSession request)
        {
            var result = new Models.ServiceResult<Session>();

            var game = await _db.Games
                .Include(g => g.Rules)
                .FirstOrDefaultAsync(g => g.Id == request.GameId);

            if (game == null)
            {
                result.Success = false;
                result.ErrorMessage = "Game not found.";
                result.StatusCode = 404;
                return result;
            }


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

            result.Success = true;
            result.Data = session;
            result.StatusCode = 200;
            return result;
        }

        public async Task<Models.ServiceResult<SessionNumberResult>> GetNextNumberAsync(int sessionId)
        {
            var result = new Models.ServiceResult<SessionNumberResult>();

            var session = await _db.Sessions
                .Include(s => s.Game)
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
            {
                result.Success = false;
                result.ErrorMessage = "Session not found.";
                result.StatusCode = 404;
                return result;
            }


            if (DateTime.UtcNow > session.EndTime)
            {
                result.Success = false;
                result.ErrorMessage = "Session has expired.";
                result.StatusCode = 400;
                return result;
            }


            var min = session.Game!.MinNumber;
            var max = session.Game!.MaxNumber;

            var random = new Random();
            int nextNum;
            bool isDuplicate;
            do
            {
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

            result.Success = true;
            result.Data = new SessionNumberResult { Number = nextNum };
            result.StatusCode = 200;
            return result;
        }


        public async Task<Models.ServiceResult<AnswerResult>> SubmitAnswerAsync(int sessionId, Models.SubmitAnswer request)
        {
            var result = new Models.ServiceResult<AnswerResult>();

            var session = await _db.Sessions
                                   .Include(s => s.Game)
                                   .ThenInclude(g => g.Rules)
                                   .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
            {
                result.Success = false;
                result.ErrorMessage = "Session not found.";
                result.StatusCode = 404;
                return result;
            }


            if (DateTime.UtcNow > session.EndTime)
            {
                result.Success = false;
                result.ErrorMessage = "Session has expired.";
                result.StatusCode = 400;
                return result;
            }


            var computedAnswer = ComputeReplacement(request.Number, session.Game!.Rules);
            bool isCorrect = string.Equals(request.UserAnswer, computedAnswer, StringComparison.OrdinalIgnoreCase);

            if (isCorrect)
                session.CorrectCount++;
            else
                session.IncorrectCount++;

            await _db.SaveChangesAsync();

            result.Success = true;
            result.Data = new AnswerResult 
            { 
                IsCorrect = isCorrect, 
                ExpectedAnswer = computedAnswer 
            };
            result.StatusCode = 200;
            return result;
        }


        public async Task<Models.ServiceResult<SessionResult>> GetSessionResultsAsync(int sessionId)
        {
            var result = new Models.ServiceResult<SessionResult>();

            var session = await _db.Sessions.FindAsync(sessionId);
            if (session == null)
            {
                result.Success = false;
                result.ErrorMessage = "Session not found.";
                result.StatusCode = 404;
                return result;
            }


            result.Success = true;
            result.Data = new SessionResult 
            { 
                CorrectCount = session.CorrectCount, 
                IncorrectCount = session.IncorrectCount 
            };
            result.StatusCode = 200;
            return result;
        }


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


            if (string.IsNullOrEmpty(result))
                result = number.ToString();

            return result;
        }
    }
}
