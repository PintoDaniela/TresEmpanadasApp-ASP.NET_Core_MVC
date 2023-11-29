namespace Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool Status { get; set; }
        public DateTime FechaAlta { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
    }
}
