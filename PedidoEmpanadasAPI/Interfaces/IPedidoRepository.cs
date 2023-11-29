using Shared.DTOs;
using Shared.Models;

namespace PedidoEmpanadasAPI.Interfaces
{
    public interface IPedidoRepository
    {
        Task<PedidoDto> HacerPedido(PedidoDto pedidoNuevo);
    }
}
