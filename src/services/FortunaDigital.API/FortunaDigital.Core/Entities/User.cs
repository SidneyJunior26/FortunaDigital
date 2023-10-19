namespace FortunaDigital.Core.Entities;

public class User : BaseEntity {
    public User(string cpf, string password, string name, int age, string phoneNumber, string email) {
        Cpf = cpf;
        Password = password;
        Name = name;
        Age = age;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public void UpdateUser(string name, int age, string phoneNumber, string email) {
        Name = name;
        Age = age;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public void SetIdForTesting(Guid newId) {
        base.SetIdForTesting(newId); // Método interno para definir o valor do Id apenas para testes
    }

    public string Cpf {
        get;
        private set;
    }
    public string Password {
        get;
        private set;
    }
    public string Name {
        get;
        private set;
    }
    public int Age {
        get;
        private set;
    }
    public string PhoneNumber {
        get;
        private set;
    }
    public string Email {
        get;
        private set;
    }
    public List<RaffleNumbers> RaffleNumbers {
        get;
        private set;
    }
}

