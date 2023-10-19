using FortunaDigital.Application.InputModels;
using FortunaDigital.Application.Services.Interfaces;
using FortunaDigital.Core.Entities;
using FortunaDigital.Infraistructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FortunaDigital.Application.Services.Implementations;

public class UserService : IUserService {
    private readonly FortunaDigitalDbContext _context;
    private readonly IAuthService _authService;

    public UserService(FortunaDigitalDbContext context, IAuthService authService) {
        _context = context;
        _authService = authService;
    }

    public async Task<Guid> CreateUser(UserInformationsInputModel newUser) {
        var user = new User(
            _authService.ComputeSha256Hash(newUser.Cpf),
            _authService.ComputeSha256Hash(newUser.Password),
            _authService.ComputeSha256Hash(newUser.Name),
            newUser.Age,
            _authService.ComputeSha256Hash(newUser.PhoneNumber),
            _authService.ComputeSha256Hash(newUser.Email));

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task DeleteUser(User user) {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllUsersList() {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetIfUserExists(string cpf, string phoneNumber, string email) {
        return await _context.Users.SingleOrDefaultAsync(u => u.Cpf == cpf ||
                                                        u.PhoneNumber == phoneNumber ||
                                                        u.Email == email);
    }

    public async Task<User> GetUserById(Guid id) {
        return await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateUser(UserInformationsInputModel userUpdated, User user) {
        user.UpdateUser(userUpdated.Name, userUpdated.Age, userUpdated.PhoneNumber, userUpdated.Email);

        await _context.SaveChangesAsync();
    }

    public async Task<string> Login(string cpf, string password) {
        var cpfEncrypted = _authService.ComputeSha256Hash(cpf);
        var passwordEncrypted = _authService.ComputeSha256Hash(password);

        var user = await _context.Users.SingleOrDefaultAsync(u => u.Cpf == cpfEncrypted && u.Password == passwordEncrypted);

        if (user == null)
            return string.Empty;

        var token = _authService.GenerateJwtToken(user.Email);

        return token;
    }
}