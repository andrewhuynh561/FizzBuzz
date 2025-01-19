using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzz.Data
{
    public class FizzBuzzDbContext : DbContext
    {

        public FizzBuzzDbContext(DbContextOptions<FizzBuzzDbContext> options)
            : base(options)
        {
        }

       

        public DbSet<Games> Games => Set<Games>();
        public DbSet<GameRule> GameRules => Set<GameRule>();
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<SessionRandomNumber> SessionRandomNumbers => Set<SessionRandomNumber>();
    }
}
