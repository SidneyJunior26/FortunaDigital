using FortunaDigital.Application.InputModels;
using FortunaDigital.Core.Entities;

namespace FortunaDigital.Application.Services.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllUsersList();
    Task<User> GetUserById(Guid id);
    Task<User> GetIfUserExists(string cpf, string phoneNumber, string email);
    Task<Guid> CreateUser(UserInformationsInputModel newUser);
    Task UpdateUser(UserInformationsInputModel userUpdated, User user);
    Task DeleteUser(User user);
    Task<string> Login(string cpf, string password);
}

