using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using PedidoEmpanadasAPI.Data;
using Shared.DTOs;
using PedidoEmpanadasAPI.Helpers;
using PedidoEmpanadasAPI.Interfaces;
using Shared.Models;

namespace PedidoEmpanadasAPI.Repositories
{    
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly PasswordHasher _passwordHasher;
        private readonly ILogger<AuthRepository> _logger;
        public AuthRepository(AppDbContext appDbContext, PasswordHasher passwordHasher, ILogger<AuthRepository> logger)
        {
            _appDbContext = appDbContext;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<User?> CreateUserAsync(UserDto newUser)
        {
            try
            {
                var (hashedPassword, passwordSalt) = _passwordHasher.HashPassword(newUser.Password);

                var user = new User
                {
                    UserName = newUser.UserName,
                    PasswordHash = hashedPassword,
                    PasswordSalt = passwordSalt,
                    FechaAlta = DateTime.UtcNow,
                    Status = true
                };

                await _appDbContext.AddAsync(user);
                var usuarioCreado = await _appDbContext.SaveChangesAsync();

                if (usuarioCreado >= 0)
                {
                    return (user);
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, User?)> CheckUserCredentialsAsync(UserDto loginUser)
        {
            try
            {
                bool validPass = false;
                var user = await _appDbContext.User.FirstOrDefaultAsync(u => u.UserName.Equals(loginUser.UserName));
                if (user != null)
                {
                    validPass = _passwordHasher.VerifyPassword(user.PasswordHash, user.PasswordSalt, loginUser.Password);
                }
                if (user == null || !validPass)
                {
                    return (false, null);
                }
                return (true, user);
            }
            catch (Exception ex)
            {
                return (false, null);
            }
        }

        public async Task<bool> ExisteUsuarioByNameAsync(string userame)
        {
            try
            {

                bool existe = await _appDbContext.User.AnyAsync(u => u.UserName.Equals(userame.Trim()));
                return existe;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }    
}
