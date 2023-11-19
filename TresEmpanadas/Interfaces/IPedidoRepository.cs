using Shared.DTOs;
using Shared.Models;

namespace TresEmpanadas.Interfaces
{
    public interface IPedidoRepository
    {
        Task<PedidoDto> HacerPedido(PedidoDto pedidoNuevo);
    }
}
