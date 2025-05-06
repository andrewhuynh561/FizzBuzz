using FizzBuzz.Models;
using FizzBuzz.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FizzBuzz.Services
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }

    public interface IGameService
    {
        Task<ServiceResult<Games>> CreateGameAsync(CreateGame request);
        Task<ServiceResult<bool>> DeleteGameAsync(int id);
        Task<ServiceResult<Games>> GetGameByIdAsync(int id);
        Task<ServiceResult<IEnumerable<Games>>> GetAllGamesAsync();
    }
}
