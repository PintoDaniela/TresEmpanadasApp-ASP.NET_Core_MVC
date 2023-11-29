using Shared.DTOs;
using Shared.Models;

namespace PedidoEmpanadasAPI.Interfaces
{
    public interface IAuthRepository
    {
        Task<(bool, User?)> CheckUserCredentialsAsync(UserDto loginUser);
        Task<User?> CreateUserAsync(UserDto newUser);
        Task<bool> ExisteUsuarioByNameAsync(String userame);
    }
}
