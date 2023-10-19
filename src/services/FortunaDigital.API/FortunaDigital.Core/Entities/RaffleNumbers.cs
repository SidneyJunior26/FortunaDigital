namespace FortunaDigital.Core.Entities;

public class RaffleNumbers : BaseEntity {
    public RaffleNumbers(Guid idUser, Guid idRaffle,
        int amount, decimal totalValue) {
        IdUser = idUser;
        IdRaffle = idRaffle;
        Amount = amount;
        TotalValue = totalValue;
    }

    public Guid IdUser {
        get;
        private set;
    }
    public User User {
        get;
        private set;
    }
    public Guid IdRaffle {
        get;
        private set;
    }
    public Raffle Raffle {
        get;
        private set;
    }
    public Guid IdPayment {
        get;
        private set;
    }
    public Payment Payment {
        get;
        private set;
    }
    public int Amount {
        get;
        private set;
    }
    public decimal TotalValue {
        get;
        private set;
    }
}

