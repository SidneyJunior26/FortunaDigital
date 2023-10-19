namespace FortunaDigital.Application.InputModels;

public record RaffleInputModel(string Description, int Active, decimal TotalValue, decimal TotalByNumber,
    int Amount, DateTime DrawDate, string Rules, int Level);