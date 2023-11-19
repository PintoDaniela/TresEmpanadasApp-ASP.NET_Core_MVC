namespace Shared.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int IdEmpanada { get; set; }
        public int IdUser { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }        
    }
}
