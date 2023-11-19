using Microsoft.AspNetCore.Mvc;
using RataEmpanada.Interfaces;
using Shared.DTOs;

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
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;

                var pedidoDto = new PedidoDto
                {
                    NombreUsuario = userName,
                    Pedido = pedidoItems
                };

                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

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
