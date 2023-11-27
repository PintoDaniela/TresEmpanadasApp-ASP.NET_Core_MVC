using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RataEmpanada.Interfaces;
using Shared.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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

            // Parsear el JSON
            JsonDocument jsonDocument = JsonDocument.Parse(token);

            var tokenValue = "";
            // Obtener el valor de la propiedad "token"
            if (jsonDocument.RootElement.TryGetProperty("token", out JsonElement tokenElement))
            {
                tokenValue = tokenElement.GetString();
                // Ahora, tokenValue contiene el valor de la propiedad "token"
            }

            if (!string.IsNullOrEmpty(token))
            {
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("Token", tokenValue);
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
