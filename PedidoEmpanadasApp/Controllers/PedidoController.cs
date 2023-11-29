using Microsoft.AspNetCore.Mvc;
using PedidoEmpanadasApp.Interfaces;
using Shared.DTOs;
using System.Security.Claims;

namespace PedidoEmpanadasApp.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoController(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        [HttpGet]
        public IActionResult RealizarPedido()
        {        

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RealizarPedido(List<DetallePedidoDto> pedidoItems)
        {
            var itemsSeleccionados = pedidoItems
                .Where(item => !string.IsNullOrEmpty(item.Empanada) && item.Cantidad >= 1)
                .ToList();
            if (itemsSeleccionados.Count > 0)
            {

                //Consulto los valores de las variables de sesión "UserName" y "Token" que creé en el login. 

                var userName = HttpContext.Session.GetString("UserName");
                var token = HttpContext.Session.GetString("Token");
                var pedidoDto = new PedidoDto
                {
                    NombreUsuario = userName,
                    Pedido = itemsSeleccionados

                };               


                var respuestaPedido = await _pedidoRepository.RealizarPedido(userName, pedidoDto, token);

                if (!string.IsNullOrEmpty(respuestaPedido))
                {
                    return View("../Home/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al realizar el pedido");
                }
            }
            return View("../Account/Login");
        }
    }
}

