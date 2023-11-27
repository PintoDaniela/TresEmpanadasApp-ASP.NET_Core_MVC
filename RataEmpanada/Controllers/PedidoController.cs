using Microsoft.AspNetCore.Mvc;
using RataEmpanada.Interfaces;
using Shared.DTOs;
using System.Security.Claims;

namespace RataEmpanada.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoController(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        [HttpGet]
        public IActionResult HacerPedido()
        {        

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HacerPedido(List<DetallePedidoDto> pedidoItems)
        {
            var itemsSeleccionados = pedidoItems
                .Where(item => !string.IsNullOrEmpty(item.Empanada) && item.Cantidad >= 1)
                .ToList();
            if (itemsSeleccionados.Count > 0)
            {
                
                var userName = HttpContext.Session.GetString("UserName");
                var token = HttpContext.Session.GetString("Token");
                var pedidoDto = new PedidoDto
                {
                    NombreUsuario = userName,
                    Pedido = itemsSeleccionados
                };

                //var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                var respuestaPedido = await _pedidoRepository.RealizarPedido(userName, pedidoDto, token);

                if (!string.IsNullOrEmpty(respuestaPedido))
                {
                    return View("PedidoExitoso", respuestaPedido);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al realizar el pedido");
                }
            }

            return View("PedidoFallido");
        }
    }
}
