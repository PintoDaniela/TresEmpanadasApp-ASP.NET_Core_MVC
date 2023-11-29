using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using PedidoEmpanadasAPI.Interfaces;

namespace PedidoEmpanadasAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoController(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        [HttpPost]
        public async Task<IActionResult> HacerPedido(PedidoDto pedidoNuevo)
        {
            var resultado = await _pedidoRepository.HacerPedido(pedidoNuevo);

            if (resultado == null)
            {
                return NotFound();
            }

            return CreatedAtAction("HacerPedido", resultado);
        }
    }
}
