namespace FortunaDigital.Core.Entities;

public class Payment : BaseEntity {
    public Payment(Guid idRaffleNumber, int paymentMethod, string qrCode,
        string pixCode, string barCode, string paymentLink, decimal totalValue)
    {
        IdRaffleNumber = idRaffleNumber;
        PaymentMethod = paymentMethod;
        QrCode = qrCode;
        PixCode = pixCode;
        BarCode = barCode;
        PaymentLink = paymentLink;
        TotalValue = totalValue;
        Paid = 0;
    }

    public Guid IdRaffleNumber {
        get;
        private set;
    }
    public RaffleNumbers RaffleNumber {
        get;
        private set;
    }
    public int PaymentMethod {
        get;
        private set;
    }
    public string QrCode {
        get;
        private set;
    }
    public string PixCode {
        get;
        private set;
    }
    public string BarCode {
        get;
        private set;
    }
    public string PaymentLink {
        get;
        private set;
    }
    public decimal TotalValue {
        get;
        private set;
    }
    public int Paid {
        get;
        private set;
    }
}

