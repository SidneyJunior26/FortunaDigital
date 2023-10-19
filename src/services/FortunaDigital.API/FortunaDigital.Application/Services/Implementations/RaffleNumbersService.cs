using FortunaDigital.Application.InputModels;
using FortunaDigital.Application.Services.Interfaces;
using FortunaDigital.Core.Entities;
using FortunaDigital.Infraistructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FortunaDigital.Application.Services.Implementations;

public class RaffleNumbersService : IRaffleNumbersService {
    private readonly FortunaDigitalDbContext _context;

    public RaffleNumbersService(FortunaDigitalDbContext context) {
        _context = context;
    }

    public async Task<Guid> CreateRaffleNumbers(RaffleNumbersInputModel inputModel) {
        var raffleNumber = new RaffleNumbers(inputModel.IdUser, inputModel.IdRaffle,
            inputModel.Amount, inputModel.TotalValue);

        await _context.RaffleNumbers.AddAsync(raffleNumber);
        await _context.SaveChangesAsync();

        return raffleNumber.Id;
    }

    public async Task DeleteRaffleNumbers(RaffleNumbers raffleNumbers) {
        _context.RaffleNumbers.Remove(raffleNumbers);

        await _context.SaveChangesAsync();
    }

    public Task<List<RaffleNumbers>> GetRaffleNumbersByRaffleId(Guid raffleId) {
        return _context.RaffleNumbers.Where(rn => rn.IdRaffle == raffleId).ToListAsync();
    }

    public Task<List<RaffleNumbers>> GetRaffleNumbersByUserId(Guid userId) {
        return _context.RaffleNumbers.Where(rn => rn.IdUser == userId).ToListAsync();
    }

    public async Task<List<RaffleNumbers>> GetRaffleNumbersList() {
        return await _context.RaffleNumbers.ToListAsync();
    }
}

