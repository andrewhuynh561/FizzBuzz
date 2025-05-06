using FizzBuzz.Models;
using FizzBuzz.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FizzBuzz.Services
{
    public interface IGameService
    {
        Task<Models.ServiceResult<Games>> CreateGameAsync(CreateGame request);
        Task<Models.ServiceResult<bool>> DeleteGameAsync(int id);
        Task<Models.ServiceResult<Games>> GetGameByIdAsync(int id);
        Task<Models.ServiceResult<IEnumerable<Games>>> GetAllGamesAsync();
    }
}
