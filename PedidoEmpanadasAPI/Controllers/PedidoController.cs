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
            var pedidoAnteriorDelDia = await _pedidoRepository.BuscarMiPedidoDelDia(pedidoNuevo.NombreUsuario);

            if (pedidoAnteriorDelDia != null)
            {
                return BadRequest("Ya hiciste tu pedido el día de hoy.");
            }
            var resultado = await _pedidoRepository.HacerPedido(pedidoNuevo);

            if (resultado == null)
            {
                return NotFound();
            }

            return CreatedAtAction("HacerPedido", resultado);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarMiPedidoDelDia(string nombreUsuario)
        {
           
            var resultado = await _pedidoRepository.BuscarMiPedidoDelDia(nombreUsuario);

            if(resultado == null)
            {
                return NotFound();
            }
            return Ok(resultado);
        }

        [HttpGet]
        [Route("ArmarPedido")]
        public async Task<IActionResult> ArmarListaParaHacerPedido()
        {
            var resultado = await _pedidoRepository.ArmarPedidoCompletoDelDia();

            if (resultado == null)
            {
                return NotFound();
            }
            return Ok(resultado);
        }

        [HttpGet]
        [Route("ListaPedidoPorUsrario")]
        public async Task<IActionResult> ListarPedidosPorUsuario()
        {
            var resultado = await _pedidoRepository.ArmarListadoPedidosDelDia();

            if (resultado == null)
            {
                return NotFound();
            }
            return Ok(resultado);
        }
    }
}
