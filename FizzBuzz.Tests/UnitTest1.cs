using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FizzBuzz.Contracts;
using FizzBuzz.Controllers;
using FizzBuzz.Models;
using FizzBuzz.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FizzBuzz.Tests
{
    public class GamesControllerTests
    {
        private readonly Mock<IGameService> _mockGameService;
        private readonly GamesController _controller;

        public GamesControllerTests()
        {
            _mockGameService = new Mock<IGameService>();
            _controller = new GamesController(_mockGameService.Object);
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

            var expectedGame = new Games
            {
                Id = 1,
                Name = "Classic FizzBuzz",
                Author = "Alice",
                Rules = new List<GameRule>
                {
                    new GameRule { Divisor = 3, ReplacementText = "Fizz" },
                    new GameRule { Divisor = 5, ReplacementText = "Buzz" }
                },
                MinNumber = 1,
                MaxNumber = 100
            };

            _mockGameService.Setup(s => s.CreateGameAsync(It.IsAny<CreateGame>()))
                .ReturnsAsync(new ServiceResult<Games> { Success = true, Data = expectedGame, StatusCode = 201 });

            // Act
            var result = await _controller.CreateGame(request);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, okResult.StatusCode);
            var response = okResult.Value as dynamic;
            Assert.NotNull(response);
            Assert.Equal(1, response.gameId);
        }


        [Fact]
        public async Task GetGameById_Success()
        {
            // Arrange
            var expectedGame = new Games
            {
                Id = 1,
                Name = "Test FizzBuzz",
                Author = "Bob"
            };

            _mockGameService.Setup(s => s.GetGameByIdAsync(1))
                .ReturnsAsync(new ServiceResult<Games> { Success = true, Data = expectedGame, StatusCode = 200 });

            // Act
            var result = await _controller.GetGameById(1);


            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var game = okResult.Value as Games;
            Assert.NotNull(game);
            Assert.Equal("Test FizzBuzz", game.Name);
        }


        [Fact]
        public async Task GetAllGames_Success()
        {
            // Arrange
            var expectedGames = new List<Games>
            {
                new Games { Id = 1, Name = "Test 1", Author = "Alice" },
                new Games { Id = 2, Name = "Test 2", Author = "Bob" }
            };


            _mockGameService.Setup(s => s.GetAllGamesAsync())
                .ReturnsAsync(new ServiceResult<IEnumerable<Games>> 
                { 
                    Success = true, 
                    Data = expectedGames, 
                    StatusCode = 200 
                });


            // Act
            var result = await _controller.GetAllGames();


            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var games = okResult.Value as IEnumerable<Games>;
            Assert.NotNull(games);
            Assert.Equal(2, games.Count());
        }


        [Fact]
        public async Task DeleteGame_Success()
        {
            // Arrange
            _mockGameService.Setup(s => s.DeleteGameAsync(1))
                .ReturnsAsync(new ServiceResult<bool> { Success = true, Data = true, StatusCode = 204 });

            // Act
            var result = await _controller.DeleteGame(1);


            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}