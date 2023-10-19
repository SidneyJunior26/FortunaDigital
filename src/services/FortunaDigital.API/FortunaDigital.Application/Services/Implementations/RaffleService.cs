using FortunaDigital.Application.InputModels;
using FortunaDigital.Application.Services.Interfaces;
using FortunaDigital.Core.Entities;
using FortunaDigital.Infraistructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FortunaDigital.Application.Services.Implementations;

public class RaffleService : IRaffleService {
    private readonly FortunaDigitalDbContext _context;

    public RaffleService(FortunaDigitalDbContext context) {
        _context = context;
    }

    public async Task<Guid> CreateRaffle(RaffleInputModel newRaffle) {
        var raffle = new Raffle(newRaffle.Description, newRaffle.Active, newRaffle.TotalValue,
            newRaffle.TotalByNumber, newRaffle.Amount, newRaffle.DrawDate, newRaffle.Rules, newRaffle.Level);

        await _context.Raffles.AddAsync(raffle);
        await _context.SaveChangesAsync();

        return raffle.Id;
    }

    public async Task DeleteRaffle(Raffle raffle) {
        _context.Raffles.Remove(raffle);

        await _context.SaveChangesAsync();
    }

    public async Task<List<Raffle>> GetActiveRafflesList() {
        return await _context.Raffles.Where(r => r.Active == 1
            && r.DrawDate >= DateTime.Now).ToListAsync();
    }

    public async Task<Raffle> GetRaffleById(Guid Id) {
        return await _context.Raffles.SingleOrDefaultAsync(r => r.Id == Id);
    }

    public async Task<List<Raffle>> GetRafflesList() {
        return await _context.Raffles.ToListAsync();
    }

    public async Task UpdateRaffle(RaffleInputModel raffleInput, Raffle raffle) {
        raffle.UpdateRaffle(raffleInput.Description, raffleInput.Active, raffleInput.TotalValue,
            raffleInput.TotalByNumber, raffleInput.Amount, raffleInput.DrawDate, raffleInput.Rules,
            raffleInput.Level);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusRaffle(Raffle raffle) {
        raffle.UpdateStatus();

        await _context.SaveChangesAsync();
    }
}

