using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RataEmpanada.Interfaces;
using Shared.DTOs;
using System.Net.Http.Headers;
using System.Text;

namespace RataEmpanada.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDto user)
        {
            var token = await _accountRepository.LoginAsync(user);
            
            if (!string.IsNullOrEmpty(token))
            {                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Token inválido, manejar el error y posiblemente redirigir a la página de inicio de sesión
                TempData["Error"] = "Invalid credentials";
                return RedirectToAction("Login");
            }
        }
    }
}
