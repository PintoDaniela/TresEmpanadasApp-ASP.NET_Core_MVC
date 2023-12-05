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
            try
            {
                var usuario = await _appDbContext.User.FirstOrDefaultAsync(u => u.UserName == pedidoNuevo.NombreUsuario);
                

                foreach (var detalle in pedidoNuevo.Pedido)
                {
                    var empanada = await _appDbContext.Empanadas.FirstOrDefaultAsync(e => e.Gusto == detalle.Empanada);
                    
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

                if (resultado > 0)
                {
                    return pedidoNuevo;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

           
        }

        public async Task<PedidoDto> BuscarMiPedidoDelDia(string userName)
        {            
            var fechaHoy = DateTime.UtcNow.Date;
            try
            {
                var pedido = await _appDbContext.Pedidos.Where(p => p.User.UserName == userName && p.Fecha.Date == fechaHoy).Include(p => p.User).Include(p => p.Empanada).ToListAsync();
                if (pedido == null || pedido.Count == 0)
                {
                    return null;
                }
                var pedidoDelDia = new PedidoDto();
                pedidoDelDia.NombreUsuario = pedido.First().User.UserName;

                foreach(var item in pedido)
                {
                    var detalle = new DetallePedidoDto()
                    {
                        Empanada = item.Empanada.Gusto,
                        Cantidad = item.Cantidad,
                    };
                    pedidoDelDia.Pedido.Add(detalle);
                }

                return pedidoDelDia;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<DetallePedidoDto>> ArmarPedidoCompletoDelDia()
        {
            var fechaHoy = DateTime.UtcNow.Date;
            try
            {
                var pedidosHoy = await _appDbContext.Pedidos
                    .Where(p => p.Fecha.Date == fechaHoy)
                    .Include(p => p.User).Include(p => p.Empanada)
                    .ToListAsync();              

                if (pedidosHoy == null || pedidosHoy.Count == 0)
                {
                    return null;
                }               

                List<DetallePedidoDto> pedidoDelDia = pedidosHoy
                   .GroupBy(p => new { p.Empanada.Id, p.Empanada.Gusto })
                   .Select(g => new DetallePedidoDto
                   {
                       Empanada = g.Key.Gusto,
                       Cantidad = g.Sum(p => p.Cantidad)
                   })
                   .ToList();

                return pedidoDelDia;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<PedidoDto>> ArmarListadoPedidosDelDia()
        {

            var fechaHoy = DateTime.UtcNow.Date;
            try
            {
                var pedidosHoy = await _appDbContext.Pedidos
                    .Where(p => p.Fecha.Date == fechaHoy)
                    .Include(p => p.User).Include(p => p.Empanada)
                    .ToListAsync();

                if (pedidosHoy == null || pedidosHoy.Count == 0)
                {
                    return null;
                }

                var pedidoDelDia = pedidosHoy
                   .GroupBy(p => new { p.Empanada.Gusto, p.User.UserName})
                   .Select(g => new
                   {
                       NombreUsuario = g.Key.UserName,
                       Empanada = g.Key.Gusto,
                       Cantidad = g.Sum(p => p.Cantidad)
                   })
                   .ToList();

                var listaPedidos = new List<PedidoDto>();

                var pedidoUsuario = new PedidoDto();
                pedidoUsuario.NombreUsuario = pedidoDelDia.First().NombreUsuario;                

                foreach (var item in pedidoDelDia)
                {
                    if(item.NombreUsuario == pedidoUsuario.NombreUsuario)
                    {
                        var detalle = new DetallePedidoDto()
                        {
                            Empanada = item.Empanada,
                            Cantidad = item.Cantidad,
                        };
                        pedidoUsuario.Pedido.Add(detalle);
                        
                    }
                    else
                    {
                        listaPedidos.Add(pedidoUsuario);

                        pedidoUsuario = new PedidoDto();
                        pedidoUsuario.NombreUsuario = item.NombreUsuario;

                        var detalle = new DetallePedidoDto()
                        {
                            Empanada = item.Empanada,
                            Cantidad = item.Cantidad,
                        };
                        pedidoUsuario.Pedido.Add(detalle);
                        
                    }
                }
                
                listaPedidos.Add(pedidoUsuario);   

                return listaPedidos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
