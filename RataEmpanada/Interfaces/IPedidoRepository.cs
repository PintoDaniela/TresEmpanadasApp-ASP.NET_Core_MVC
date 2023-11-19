using Shared.DTOs;

namespace RataEmpanada.Interfaces
{
    public interface IPedidoRepository
    {
        Task<string> RealizarPedido(string userName, PedidoDto pedido, string token);
    }
}
