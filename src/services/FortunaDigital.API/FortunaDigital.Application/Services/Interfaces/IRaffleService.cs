using FortunaDigital.Application.InputModels;
using FortunaDigital.Core.Entities;

namespace FortunaDigital.Application.Services.Interfaces;

public interface IRaffleService {
    Task<List<Raffle>> GetRafflesList();
    Task<List<Raffle>> GetActiveRafflesList();
    Task<Raffle> GetRaffleById(Guid id);
    Task<Guid> CreateRaffle(RaffleInputModel newRaffle);
    Task UpdateRaffle(RaffleInputModel raffleInput, Raffle raffle);
    Task UpdateStatusRaffle(Raffle raffle);
    Task DeleteRaffle(Raffle raffle);
}