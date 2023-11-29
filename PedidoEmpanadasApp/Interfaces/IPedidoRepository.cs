using Shared.DTOs;

namespace PedidoEmpanadasApp.Interfaces
{
    public interface IPedidoRepository
    {
        Task<string> RealizarPedido(string userName, PedidoDto pedido, string token);
    }
}
