using FortunaDigital.API.Controllers;
using FortunaDigital.Application.Services.Interfaces;
using FortunaDigital.Application.ViewModels;
using FortunaDigital.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FortunaDigital.Test;

public class RaffleTests {
    [Fact]
    public async Task GetRafflesList_ReturnsOkWithRaffles() {
        // Arrange
        var raffles = new List<Raffle>
        {
            new Raffle("Raffle 1", 1, 100.0m, 10.0m, 100, DateTime.Now, "Rules 1", 1),
            new Raffle("Raffle 2", 1, 150.0m, 12.0m, 150, DateTime.Now, "Rules 2", 2)
        };

        var raffleServiceMock = new Mock<IRaffleService>();
        raffleServiceMock.Setup(service => service.GetRafflesList()).ReturnsAsync(raffles);

        var loggerMock = new Mock<ILogger<RaffleController>>();
        var controller = new RaffleController(raffleServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetRafflesList();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<List<Raffle>>(okResult.Value);
        Assert.Equal(raffles, model);
    }

    [Fact]
    public async Task GetRafflesList_ReturnsNotFound() {
        // Arrange
        var raffles = new List<Raffle>(); // Empty list

        var raffleServiceMock = new Mock<IRaffleService>();
        raffleServiceMock.Setup(service => service.GetRafflesList()).ReturnsAsync(raffles);

        var loggerMock = new Mock<ILogger<RaffleController>>();
        var controller = new RaffleController(raffleServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetRafflesList();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorViewModel = Assert.IsType<ErrorViewModel>(notFoundResult.Value);
        Assert.Equal("Nenhum sorteio encontrado", errorViewModel.UserResponse);
    }

    [Fact]
    public async Task GetRafflesList_ReturnsBadRequestOnException() {
        // Arrange
        var exceptionMessage = "Test exception message";
        var raffleServiceMock = new Mock<IRaffleService>();
        raffleServiceMock.Setup(service => service.GetRafflesList()).ThrowsAsync(new Exception(exceptionMessage));

        var loggerMock = new Mock<ILogger<RaffleController>>();
        var controller = new RaffleController(raffleServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetRafflesList();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var errorViewModel = Assert.IsType<ErrorViewModel>(badRequestResult.Value);
        Assert.Equal("Ocorreu um erro ao consultar os sorteios.", errorViewModel.UserResponse);
        Assert.Equal(exceptionMessage, errorViewModel.InternalError);
    }

    [Fact]
    public async Task GetActiveRafflesList_ReturnsOkWithRaffles() {
        // Arrange
        var raffles = new List<Raffle>
        {
            new Raffle("Raffle 1", 1, 100.0m, 10.0m, 100, DateTime.Now, "Rules 1", 1),
            new Raffle("Raffle 2", 1, 150.0m, 12.0m, 150, DateTime.Now, "Rules 2", 2)
        };

        var raffleServiceMock = new Mock<IRaffleService>();
        raffleServiceMock.Setup(service => service.GetActiveRafflesList()).ReturnsAsync(raffles);

        var loggerMock = new Mock<ILogger<RaffleController>>();
        var controller = new RaffleController(raffleServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetActiveRafflesList();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<List<Raffle>>(okResult.Value);
        Assert.Equal(raffles, model);
    }

    [Fact]
    public async Task GetActiveRafflesList_ReturnsNotFound() {
        // Arrange
        var raffles = new List<Raffle>(); // Empty list

        var raffleServiceMock = new Mock<IRaffleService>();
        raffleServiceMock.Setup(service => service.GetActiveRafflesList()).ReturnsAsync(raffles);

        var loggerMock = new Mock<ILogger<RaffleController>>();
        var controller = new RaffleController(raffleServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetActiveRafflesList();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorViewModel = Assert.IsType<ErrorViewModel>(notFoundResult.Value);
        Assert.Equal("Nenhum sorteio ativo encontrado", errorViewModel.UserResponse);
    }

    [Fact]
    public async Task GetRaffleById_ReturnsOkWithRaffle() {
        // Arrange
        var raffleId = Guid.NewGuid(); // Gere um novo Guid válido

        var raffle = new Raffle("Raffle 1", 1, 100.0m, 10.0m, 100, DateTime.Now, "Rules 1", 1);
        raffle.SetIdForTesting(raffleId);

        var raffleServiceMock = new Mock<IRaffleService>();
        raffleServiceMock.Setup(service => service.GetRaffleById(raffleId)).ReturnsAsync(raffle);

        var loggerMock = new Mock<ILogger<RaffleController>>();
        var controller = new RaffleController(raffleServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetRaffleById(raffleId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<Raffle>(okResult.Value);
        Assert.Equal(raffleId, model.Id);
    }

    [Fact]
    public async Task GetRaffleById_ReturnsNotFound() {
        // Arrange
        var raffleId = Guid.NewGuid();

        var raffleServiceMock = new Mock<IRaffleService>();
        raffleServiceMock.Setup(service => service.GetRaffleById(raffleId)).ReturnsAsync((Raffle)null);

        var loggerMock = new Mock<ILogger<RaffleController>>();
        var controller = new RaffleController(raffleServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetRaffleById(raffleId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorViewModel = Assert.IsType<ErrorViewModel>(notFoundResult.Value);
        Assert.Equal("Nenhum sorteio ativo encontrado", errorViewModel.UserResponse);
    }
}