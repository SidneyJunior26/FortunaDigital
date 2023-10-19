namespace FortunaDigital.Core.Entities;

public abstract class BaseEntity {
    public BaseEntity() {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    protected internal void SetIdForTesting(Guid newId) {
        Id = newId; // Método interno para definir o valor do Id apenas para testes
    }

    public Guid Id {
        get;
        private set;
    }
    public DateTime CreatedAt {
        get;
        private set;
    }
    public DateTime UpdatedAt {
        get;
        private set;
    }
}

