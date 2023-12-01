using System.Text.Json.Serialization;

namespace Shared.DTOs
{
    public class DetallePedidoDto
    {
        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }
        [JsonPropertyName("empanada")]
        public string Empanada { get; set; }
        
    }
}
