using Microsoft.EntityFrameworkCore;
using PedidoEmpanadasAPI.Data;
using Shared.DTOs;
using PedidoEmpanadasAPI.Interfaces;
using Shared.Models;

namespace PedidoEmpanadasAPI.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _appDbContext;

        public PedidoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<PedidoDto> HacerPedido(PedidoDto pedidoNuevo)
        {
            var pedidoCompleto = new PedidoDto();

            var usuario = await _appDbContext.User.FirstOrDefaultAsync(u => u.UserName == pedidoNuevo.NombreUsuario);
            //int IdUsuario = usuario.Id;

            foreach(var detalle in pedidoNuevo.Pedido)
            {
                var empanada = await _appDbContext.Empanadas.FirstOrDefaultAsync(e => e.Gusto == detalle.Empanada);
               // int idEmpanada = empanada.Id;
                int cantidad = detalle.Cantidad;

                var pedido = new Pedido()
                {
                    User = usuario,
                    Empanada = empanada,
                    Cantidad = cantidad,
                    Fecha = DateTime.UtcNow
                };

                await _appDbContext.AddAsync(pedido);
            }
            
            int resultado = await _appDbContext.SaveChangesAsync();

            if(resultado > 0)
            {
                return pedidoNuevo;
            }

            return null;            
        }
    }
}
