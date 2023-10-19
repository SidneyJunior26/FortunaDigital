using FortunaDigital.Application.InputModels;
using FortunaDigital.Application.Services.Interfaces;
using FortunaDigital.Application.ViewModels;
using FortunaDigital.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FortunaDigital.API.Controllers;

[Route("v1/[controller]")]
public class RaffleController : ControllerBase {
    private readonly IRaffleService _raffleService;
    private readonly ILogger<RaffleController> _logger;

    public RaffleController(IRaffleService raffleService, ILogger<RaffleController> logger) {
        _raffleService = raffleService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize("employee")]
    public async Task<IActionResult> GetRafflesList() {
        List<Raffle> raffles;

        try {
            raffles = await _raffleService.GetRafflesList();
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar consultar os sorteios - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao consultar os sorteios.", ex.Message));
        }

        if (!raffles.Any())
            return NotFound(new ErrorViewModel("Nenhum sorteio encontrado", ""));

        return Ok(raffles);
    }

    [HttpGet("actives-list")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActiveRafflesList() {
        List<Raffle> raffles;

        try {
            raffles = await _raffleService.GetActiveRafflesList();
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar consultar os sorteios ativos - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao consultar os sorteios ativos.", ex.Message));
        }

        if (!raffles.Any())
            return NotFound(new ErrorViewModel("Nenhum sorteio ativo encontrado", ""));

        return Ok(raffles);
    }

    [HttpGet("{Id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRaffleById(Guid Id) {
        Raffle raffle;

        try {
            raffle = await _raffleService.GetRaffleById(Id);
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar consultar os sorteios ativos - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao consultar os sorteios ativos.", ex.Message));
        }

        if (raffle == null)
            return NotFound(new ErrorViewModel("Nenhum sorteio ativo encontrado", ""));

        return Ok(raffle);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateRaffle(RaffleInputModel newRaffle) {
        Guid idRaffle;

        try {
            idRaffle = await _raffleService.CreateRaffle(newRaffle);
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar consultar os sorteios ativos - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao consultar os sorteios ativos.", ex.Message));
        }

        return Ok(idRaffle);
    }

    [HttpPut("{idRaffle}")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateRaffle(Guid idRaffle, RaffleInputModel newRaffle) {
        Raffle raffle;

        try {
            raffle = await _raffleService.GetRaffleById(idRaffle);

            if (raffle == null)
                return NotFound(new ErrorViewModel("Nenhum Sorteio encontrado.", ""));

        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar consultar o sorteio com ID '{idRaffle}' - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao consultar o Sorteio.", ex.Message));
        }


        try {
            await _raffleService.UpdateRaffle(newRaffle, raffle);
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar atualizar o sorteio - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao atualizar o sorteio.", ex.Message));
        }

        return NoContent();
    }

    [HttpPut("update-status/{idRaffle}")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateStatusRaffle(Guid idRaffle) {
        Raffle raffle;

        try {
            raffle = await _raffleService.GetRaffleById(idRaffle);

            if (raffle == null)
                return NotFound(new ErrorViewModel("Nenhum Sorteio encontrado.", ""));

        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar consultar o sorteio com ID '{idRaffle}' - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao consultar o Sorteio.", ex.Message));
        }


        try {
            await _raffleService.UpdateStatusRaffle(raffle);
        } catch (Exception ex) {
            _logger.LogError($"Ocorreu um erro ao tentar atualizar o sorteio - {ex.Message}");

            return BadRequest(new ErrorViewModel("Ocorreu um erro ao atualizar o sorteio.", ex.Message));
        }

        return NoContent();
    }
}

