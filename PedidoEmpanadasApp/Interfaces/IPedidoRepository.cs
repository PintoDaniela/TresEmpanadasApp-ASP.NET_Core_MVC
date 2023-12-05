using Shared.DTOs;
using Shared.Models;

namespace PedidoEmpanadasApp.Interfaces
{
    public interface IPedidoRepository
    {
        Task<string> RealizarPedido(string userName, PedidoDto pedido, string token);
        Task<PedidoDto> MostrarPedido(string userName, string token);
        Task<IEnumerable<DetallePedidoDto>> ArmarPedido(string userName, string token);
    }
}
