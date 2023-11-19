using Shared.DTOs;

namespace RataEmpanada.Interfaces
{
    public interface IAccountRepository
    {
        Task<string> LoginAsync(UserDto user);
    }
}
