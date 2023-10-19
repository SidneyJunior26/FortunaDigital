namespace FortunaDigital.Core.Entities;

public class Raffle : BaseEntity {
    public Raffle(string description, int active, decimal totalValue, decimal totalByNumber, int amount, DateTime drawDate,
        string rules, int level) {
        Description = description;
        Active = active;
        TotalValue = totalValue;
        TotalByNumber = totalByNumber;
        Amount = amount;
        DrawDate = drawDate;
        Rules = rules;
        Level = level;
    }

    public void UpdateRaffle(string description, int active, decimal totalValue, decimal totalByNumber, int amount,
        DateTime drawDate, string rules, int level) {
        Description = description;
        Active = active;
        TotalValue = totalValue;
        TotalByNumber = totalByNumber;
        Amount = amount;
        DrawDate = drawDate;
        Rules = rules;
        Level = level;
    }

    public void UpdateStatus() {
        Active = Active == 1 ? 0 : 1;
    }

    public void SetIdForTesting(Guid newId) {
        base.SetIdForTesting(newId); // Método interno para definir o valor do Id apenas para testes
    }

    public string Description {
        get;
        private set;
    }
    public int Active {
        get;
        private set;
    }
    public decimal TotalValue {
        get;
        private set;
    }
    public decimal TotalByNumber {
        get;
        private set;
    }
    public int Amount {
        get;
        private set;
    }
    public DateTime DrawDate {
        get;
        private set;
    }
    public string Rules {
        get;
        private set;
    }
    public int Level {
        get;
        private set;
    }
    public List<RaffleNumbers> RaffleNumbers {
        get;
        private set;
    }
}