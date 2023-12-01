using System.Text.Json.Serialization;

namespace Shared.DTOs
{
    public class PedidoDto
    {
        [JsonPropertyName("nombreUsuario")]
        public string NombreUsuario { get; set; }
        [JsonPropertyName("pedido")]
        public List<DetallePedidoDto> Pedido { get; set; } = new List<DetallePedidoDto>();
    }
}
