using Shared.DTOs;
using Shared.Models;

namespace PedidoEmpanadasAPI.Interfaces
{
    public interface IPedidoRepository
    {
        Task<PedidoDto> HacerPedido(PedidoDto pedidoNuevo);
        Task<PedidoDto> BuscarMiPedidoDelDia(string userName);
        Task<IEnumerable<DetallePedidoDto>> ArmarPedidoCompletoDelDia();
        Task<IEnumerable<PedidoDto>> ArmarListadoPedidosDelDia();
    }
}
