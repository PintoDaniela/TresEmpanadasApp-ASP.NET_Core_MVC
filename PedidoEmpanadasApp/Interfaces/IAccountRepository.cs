using Shared.DTOs;

namespace PedidoEmpanadasApp.Interfaces
{
    public interface IAccountRepository
    {
        Task<string> LoginAsync(UserDto user);
    }
}
