namespace FortunaDigital.Application.InputModels;

public record RaffleNumbersInputModel(Guid IdUser, Guid IdRaffle, int Amount, decimal TotalValue);