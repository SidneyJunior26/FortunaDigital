using FortunaDigital.Application.InputModels;
using FortunaDigital.Core.Entities;

namespace FortunaDigital.Application.Services.Interfaces;

public interface IRaffleNumbersService {
    Task<List<RaffleNumbers>> GetRaffleNumbersList();
    Task<List<RaffleNumbers>> GetRaffleNumbersByRaffleId(Guid raffleId);
    Task<List<RaffleNumbers>> GetRaffleNumbersByUserId(Guid userId);
    Task<Guid> CreateRaffleNumbers(RaffleNumbersInputModel inputModel);
    Task DeleteRaffleNumbers(RaffleNumbers raffleNumbers);
}

