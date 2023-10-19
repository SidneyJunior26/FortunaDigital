using FortunaDigital.API.Controllers;
using FortunaDigital.Application.InputModels;
using FortunaDigital.Application.Services.Interfaces;
using FortunaDigital.Application.ViewModels;
using FortunaDigital.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FortunaDigital.Test;

public class UserTests {
    [Fact]
    public async Task GetAllList_ReturnsOkWithUsers() {
        // Arrange
        var users = new List<User> {
            new User("12345678910", "1234", "Teste", 27, "11976789654", "teste@teste.com.br"),
            new User("12345678911", "1234", "Teste2", 28, "11976789655", "teste2@teste2.com.br")
        };

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(service => service.GetAllUsersList()).ReturnsAsync(users);

        var loggerMock = new Mock<ILogger<UserController>>();
        var controller = new UserController(userServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetAllList();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<List<User>>(okResult.Value);
        Assert.Equal(users, model);
    }

    [Fact]
    public async Task GetUserById_ReturnsOkWithUser() {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User("12345678910", "1234", "Teste", 27, "11976789654", "teste@teste.com.br");
        user.SetIdForTesting(userId);

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(service => service.GetUserById(userId)).ReturnsAsync(user);

        var loggerMock = new Mock<ILogger<UserController>>();
        var controller = new UserController(userServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetUserById(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<User>(okResult.Value);
        Assert.Equal(userId, model.Id);
    }

    [Fact]
    public async Task GetUserById_ReturnsNotFound() {
        // Arrange
        var userId = Guid.NewGuid();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(service => service.GetUserById(userId)).ReturnsAsync((User)null);

        var loggerMock = new Mock<ILogger<UserController>>();
        var controller = new UserController(userServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetUserById(userId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorViewModel = Assert.IsType<ErrorViewModel>(notFoundResult.Value);
        Assert.Equal("Nenhum usuário com este ID encontrado", errorViewModel.UserResponse);
    }

    [Fact]
    public async Task CreateUser_ReturnsOkWithUserId() {
        // Arrange
        var newUserInput = new UserInformationsInputModel("12345678910", "1234", "Teste", 27, "11976789654", "teste@teste.com.br");
        var newUserId = Guid.NewGuid();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(service => service.GetIfUserExists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((User)null);
        userServiceMock.Setup(service => service.CreateUser(newUserInput)).ReturnsAsync(newUserId);

        var loggerMock = new Mock<ILogger<UserController>>();
        var controller = new UserController(userServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.CreateUser(newUserInput);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var id = Assert.IsType<Guid>(okResult.Value);
        Assert.Equal(newUserId, id);
    }

    [Fact]
    public async Task CreateUser_ReturnsBadRequestWhenUserExists() {
        // Arrange
        var newUserInput = new UserInformationsInputModel("12345678910", "1234", "Teste", 27, "11976789654", "teste@teste.com.br");
        var existingUser = new User("12345678910", "1234", "Teste", 27, "11976789654", "teste@teste.com.br");

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(service => service.GetIfUserExists(newUserInput.Cpf, newUserInput.PhoneNumber, newUserInput.Email))
            .ReturnsAsync(existingUser); // Simula a existência do usuário, portanto, CreateUser não deve ser chamado

        var loggerMock = new Mock<ILogger<UserController>>();
        var controller = new UserController(userServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.CreateUser(newUserInput);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var errorViewModel = Assert.IsType<ErrorViewModel>(badRequestResult.Value);
        Assert.Equal("Existe algum cadastro com este CPF, Telefone ou Email. Favor confirmar os dados", errorViewModel.UserResponse);
    }

    [Fact]
    public async Task UpdateUser_ReturnsNoContent() {
        // Arrange
        var userId = Guid.NewGuid();
        var newUserInput = new UserInformationsInputModel("12345678910", "1234", "Teste", 27, "11976789654", "teste@teste.com.br");
        var existingUser = new User("12345678910", "1234", "Teste", 27, "11976789654", "teste@teste.com.br");
        existingUser.SetIdForTesting(userId);

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(service => service.GetUserById(userId)).ReturnsAsync(existingUser);

        var loggerMock = new Mock<ILogger<UserController>>();
        var controller = new UserController(userServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.UpdateUser(userId, newUserInput);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateUser_ReturnsNotFound() {
        // Arrange
        var userId = Guid.NewGuid();
        var newUserInput = new UserInformationsInputModel ("12345678910", "1234", "Teste", 27, "11976789654", "teste@teste.com.br" );

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(service => service.GetUserById(userId)).ReturnsAsync((User)null);

        var loggerMock = new Mock<ILogger<UserController>>();
        var controller = new UserController(userServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.UpdateUser(userId, newUserInput);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorViewModel = Assert.IsType<ErrorViewModel>(notFoundResult.Value);
        Assert.Equal("Nenhum usuário encontrado.", errorViewModel.UserResponse);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent() {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new User("12345678910", "1234", "Teste", 27, "11976789654", "teste@teste.com.br");

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(service => service.GetUserById(userId)).ReturnsAsync(existingUser);

        var loggerMock = new Mock<ILogger<UserController>>();
        var controller = new UserController(userServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.DeleteUser(userId);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContentWhenUserExists() {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User("12345678910", "1234", "Teste", 27, "11976789654", "teste@teste.com.br");

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(service => service.GetUserById(userId)).ReturnsAsync(user);

        var loggerMock = new Mock<ILogger<UserController>>();
        var controller = new UserController(userServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.DeleteUser(userId);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
    }


    [Fact]
    public async Task Login_ReturnsOkWithToken() {
        // Arrange
        var loginInputModel = new LoginInputModel("12345678910", "1234");
        var token = "your_generated_token_here";

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(service => service.Login(loginInputModel.cpf, loginInputModel.password)).ReturnsAsync(token);

        var loggerMock = new Mock<ILogger<UserController>>();
        var controller = new UserController(userServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.Login(loginInputModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tokenResponse = Assert.IsType<string>(okResult.Value);
        Assert.Equal(token, tokenResponse);
    }

    [Fact]
    public async Task Login_ReturnsBadRequestWhenTokenIsNull() {
        // Arrange
        var loginInputModel = new LoginInputModel("12345678910", "1234");
        string token = null;

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(service => service.Login(loginInputModel.cpf, loginInputModel.password)).ReturnsAsync(token);

        var loggerMock = new Mock<ILogger<UserController>>();
        var controller = new UserController(userServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.Login(loginInputModel);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var errorViewModel = Assert.IsType<ErrorViewModel>(badRequestResult.Value);
        Assert.Equal("Suas informações de login são inválidas. Verifique seu CPF e senha.", errorViewModel.UserResponse);
    }
}

