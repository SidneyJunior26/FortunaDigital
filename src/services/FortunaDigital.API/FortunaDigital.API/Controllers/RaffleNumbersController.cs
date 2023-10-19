using FortunaDigital.Application.InputModels;
using FortunaDigital.Application.Services.Interfaces;
using FortunaDigital.Application.ViewModels;
using FortunaDigital.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FortunaDigital.API.Controllers;

[Route("v1/[controller]")]
public class RaffleNumbersController : ControllerBase {
    private readonly IRaffleNumbersService _raffleNumbersService;
    private readonly ILogger<RaffleNumbersController> _logger;

    public RaffleNumbersController(IRaffleNumbersService raffleNumbersService, ILogger<RaffleNumbersController> logger) {
        _raffleNumbersService = raffleNumbersService;
        _logger = logger;
    }

    [HttpGet("{raffleId}")]
    [Authorize]
    public async Task<IActionResult> GetAllByRaffleId(Guid raffleId) {
        List<RaffleNumbers> raffleNumbers;
        try {
            raffleNumbers = await _raffleNumbersService.GetRaffleNumbersByRaffleId(raffleId);
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar consultar os Números do Sorteio - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao consultar os números do sorteio.", ex.Message));
        }

        if (raffleNumbers == null)
            return NotFound(new ErrorViewModel("Ocorreu um erro ao consultar os números do sorteio.", ""));

        return Ok(raffleNumbers);
    }

    [HttpGet("{userId}")]
    [Authorize]
    public async Task<IActionResult> GetByUserId(Guid userId) {
        List<RaffleNumbers> raffleNumbers;

        try {
            raffleNumbers = await _raffleNumbersService.GetRaffleNumbersByUserId(userId);
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar consultar os Números do Sorteio do usuário - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao consultar os números do sorteio do usuário.", ex.Message));
        }

        if (raffleNumbers == null)
            return NotFound(new ErrorViewModel("Ocorreu um erro ao consultar os números do sorteio.", ""));

        return Ok(raffleNumbers);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateRaffleNumbers(RaffleNumbersInputModel raffleNumbers) {
        Guid raffleNumbersId;

        try {
            raffleNumbersId = await _raffleNumbersService.CreateRaffleNumbers(raffleNumbers);
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar cadastrar os Números do Sorteio - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao cadastrar os números do sorteio.", ex.Message));
        }

        return Ok(raffleNumbersId);
    }
}

