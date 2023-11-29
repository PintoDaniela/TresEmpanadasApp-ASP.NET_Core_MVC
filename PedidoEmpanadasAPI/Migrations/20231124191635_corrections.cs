using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PedidoEmpanadasAPI.Migrations
{
    public partial class corrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdEmpanada",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Pedidos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdEmpanada",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUser",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
