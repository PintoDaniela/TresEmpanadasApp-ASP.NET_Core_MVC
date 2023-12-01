using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PedidoEmpanadasApp.Interfaces;
using Shared.DTOs;
using Shared.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PedidoEmpanadasApp.Controllers
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

            if (token == null)
            {
                TempData["Error"] = "Credenciales inválidas";
                return RedirectToAction("Login");
            }
            
            JsonDocument jsonDocument = JsonDocument.Parse(token);

            var tokenValue = "";            
            if (jsonDocument.RootElement.TryGetProperty("token", out JsonElement tokenElement))
            {
                tokenValue = tokenElement.GetString();                
            }

            if (!string.IsNullOrEmpty(tokenValue))

            {
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("Token", tokenValue);
                return RedirectToAction("Index", "Home");
            }
            else
            {                
                TempData["Error"] = "Credenciales inválidas";
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("UserName");
            return View();
        }
    }
}
