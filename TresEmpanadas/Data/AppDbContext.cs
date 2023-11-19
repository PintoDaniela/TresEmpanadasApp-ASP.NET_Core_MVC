using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace TresEmpanadas.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Empanada> Empanadas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Empanada>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
