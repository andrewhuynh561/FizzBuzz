using FizzBuzz.Data;
using FizzBuzz.Models;
using FizzBuzz.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FizzBuzz.Services
{
    public class GameService : IGameService
    {
        private readonly FizzBuzzDbContext _db;

        public GameService(FizzBuzzDbContext db)
        {
            _db = db;
        }

        public async Task<ServiceResult<Games>> CreateGameAsync(CreateGame request)
        {
            var result = new ServiceResult<Games>();

            if (request.Rules == null || !request.Rules.Any())
            {
                result.Success = false;
                result.ErrorMessage = "At least one rule is required.";
                result.StatusCode = 400;
                return result;
            }


            if (request.Rules.Any(r => r.Divisor <= 0))
            {
                result.Success = false;
                result.ErrorMessage = "Divisor must be positive.";
                result.StatusCode = 400;
                return result;
            }


            if (request.MinNumber < 1 || request.MaxNumber <= request.MinNumber)
            {
                result.Success = false;
                result.ErrorMessage = "Invalid range number.";
                result.StatusCode = 400;
                return result;
            }


            try
            {
                var game = new Games
                {
                    Name = request.Name,
                    Author = request.Author,
                    CreatedAt = DateTime.UtcNow,
                    MinNumber = request.MinNumber,
                    MaxNumber = request.MaxNumber
                };


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


                result.Success = true;
                result.Data = game;
                result.StatusCode = 201; // Created
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "An error occurred while creating the game.";
                result.StatusCode = 500;
                // Log the exception here if needed
            }

            return result;
        }


        public async Task<ServiceResult<bool>> DeleteGameAsync(int id)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var game = await _db.Games.FindAsync(id);
                if (game == null)
                {
                    result.Success = false;
                    result.StatusCode = 404;
                    result.ErrorMessage = "Game not found.";
                    result.Data = false;
                    return result;
                }


                _db.Games.Remove(game);
                await _db.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                result.StatusCode = 204; // No Content
            }
            catch (Exception)
            {
                result.Success = false;
                result.ErrorMessage = "An error occurred while deleting the game.";
                result.StatusCode = 500;
                result.Data = false;
            }


            return result;
        }


        public async Task<ServiceResult<Games>> GetGameByIdAsync(int id)
        {
            var result = new ServiceResult<Games>();

            try
            {
                var game = await _db.Games
                                  .Include(g => g.Rules)
                                  .FirstOrDefaultAsync(g => g.Id == id);

                if (game == null)
                {
                    result.Success = false;
                    result.StatusCode = 404;
                    result.ErrorMessage = "Game not found.";
                    return result;
                }


                result.Success = true;
                result.Data = game;
                result.StatusCode = 200;
            }
            catch (Exception)
            {
                result.Success = false;
                result.ErrorMessage = "An error occurred while retrieving the game.";
                result.StatusCode = 500;
            }


            return result;
        }


        public async Task<ServiceResult<IEnumerable<Games>>> GetAllGamesAsync()
        {
            var result = new ServiceResult<IEnumerable<Games>>();

            try
            {
                var games = await _db.Games
                                  .Include(g => g.Rules)
                                  .ToListAsync();

                result.Success = true;
                result.Data = games;
                result.StatusCode = 200;
            }
            catch (Exception)
            {
                result.Success = false;
                result.ErrorMessage = "An error occurred while retrieving games.";
                result.StatusCode = 500;
            }


            return result;
        }
    }
}
