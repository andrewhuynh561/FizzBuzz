using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FizzBuzz.Contracts;
using FizzBuzz.Api.Controllers;
using FizzBuzz.Data;
using FizzBuzz.Models;
using Microsoft.AspNetCore.Mvc;

namespace FizzBuzz.Tests
{
    public class GamesControllerTests : IDisposable
    {
        private readonly FizzBuzzDbContext _dbContext;
        private readonly GamesController _controller;

        public GamesControllerTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<FizzBuzzDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new FizzBuzzDbContext(options);
            _controller = new GamesController(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task CreateGame_Success()
        {
            // Arrange
            var request = new CreateGame
            {
                Name = "Classic FizzBuzz",
                Author = "Alice",
                Rules = new List<CreateRule>
                {
                    new CreateRule { Divisor = 3, ReplacementText = "Fizz" },
                    new CreateRule { Divisor = 5, ReplacementText = "Buzz" }
                }
            };

            // Act
            var result = await _controller.CreateGame(request);

            //// Assert
            //var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            //Assert.Equal(200, createdResult.StatusCode);

            var gamesInDb = await _dbContext.Games.Include(g => g.Rules).ToListAsync();
            Assert.Single(gamesInDb);
            Assert.Equal("Classic FizzBuzz", gamesInDb[0].Name);
            Assert.Equal("Alice", gamesInDb[0].Author);
            Assert.Equal(2, gamesInDb[0].Rules.Count);
        }

        [Fact]
        public async Task GetGames_Success()
        {
            // Arrange
            var game = new Games
            {
                Name = "Test FizzBuzz",
                Author = "Bob",
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetAllGames();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var games = Assert.IsType<List<Games>>(okResult.Value);
            Assert.Single(games);
            Assert.Equal("Test FizzBuzz", games[0].Name);
        }
    }
}