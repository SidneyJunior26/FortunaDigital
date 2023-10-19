namespace FortunaDigital.Application.Services.Interfaces;

public interface IAuthService {
    string ComputeSha256Hash(string propertie);
    string GenerateJwtToken(string email);
}