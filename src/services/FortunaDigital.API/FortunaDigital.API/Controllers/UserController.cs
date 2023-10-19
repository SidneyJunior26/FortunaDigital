using FortunaDigital.Application.InputModels;
using FortunaDigital.Application.Services.Interfaces;
using FortunaDigital.Application.ViewModels;
using FortunaDigital.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FortunaDigital.API.Controllers;

[Route("v1/[controller]")]
public class UserController : ControllerBase {
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger) {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "client, freelancer")]
    public async Task<IActionResult> GetAllList() {
        var users = new List<User>();

        try {
            users = await _userService.GetAllUsersList();
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar consultar os usuários - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao consultar os usuários.", ex.Message));
        }

        if (!users.Any())
            return NotFound(new ErrorViewModel("Nenhum usuário encontrado", ""));

        return Ok(users);
    }

    [HttpGet("{Id}")]
    [Authorize(Roles = "client, freelancer")]
    public async Task<IActionResult> GetUserById(Guid Id) {
        User user;

        try {
            user = await _userService.GetUserById(Id);
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar consultar o usuário com ID '{Id}' - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao consultar o usuário.", ex.Message));
        }

        if (user == null)
            return NotFound(new ErrorViewModel("Nenhum usuário com este ID encontrado", Id.ToString()));

        return Ok(user);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(UserInformationsInputModel NewUser) {
        Guid newUserId;

        try {
            var user = _userService.GetIfUserExists(NewUser.Cpf, NewUser.PhoneNumber, NewUser.Email);

            if (user != null) {
                _logger.LogWarning($"Usuário já cadastrado com mesmo CPF {NewUser.Cpf} ou Telefone {NewUser.PhoneNumber} ou Email {NewUser.Email}");

                return BadRequest(new ErrorViewModel("Existe algum cadastro com este CPF, Telefone ou Email. Favor confirmar os dados", ""));
            }

            newUserId = await _userService.CreateUser(NewUser);
        } catch (Exception ex) {
            _logger.LogCritical($"Ocorreu um erro ao cadastrar o usuário - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao cadastrar o usuário. Tente novamente mais tarde.", ex.Message));
        }

        return Ok(newUserId);
    }

    [HttpPut]
    [Authorize(Roles = "client, freelancer")]
    public async Task<IActionResult> UpdateUser(Guid UserId, UserInformationsInputModel newUser) {
        User user;

        try {
            user = await _userService.GetUserById(UserId);

            if (user == null)
                return NotFound(new ErrorViewModel("Nenhum usuário encontrado.", ""));

        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar consultar o usuário com ID '{UserId}' - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao consultar o usuário.", ex.Message));
        }

        try {
            await _userService.UpdateUser(newUser, user);
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao atualizar o usuário - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao atualizar o usuário. Tente novamente mais tarde.", ex.Message));
        }

        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = "client, freelancer")]
    public async Task<IActionResult> DeleteUser(Guid UserId) {
        User user;

        try {
            user = await _userService.GetUserById(UserId);
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar consultar o usuário com ID '{UserId}' - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao consultar o usuário.", ex.Message));
        }

        try {
            await _userService.DeleteUser(user);
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao apagar o usuário - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao apagar o usuário. Tente novamente mais tarde.", ex.Message));
        }

        return NoContent();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginInputModel loginInputModel) {
        string token;

        try {
            token = await _userService.Login(loginInputModel.cpf, loginInputModel.password);
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao realizar o login - ", ex.Message);

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao realizaro login.", ex.Message));
        }

        if (String.IsNullOrEmpty(token))
            return BadRequest(new ErrorViewModel("\"Suas informações de login são inválidas. Verifique seu CPF e senha.", ""));

        return Ok(new { token = token });
    }
}