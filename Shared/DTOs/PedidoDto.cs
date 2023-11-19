namespace Shared.DTOs
{
    public class PedidoDto
    {
        public string NombreUsuario { get; set; }
        public List<DetallePedidoDto> Pedido { get; set; }
    }
}
