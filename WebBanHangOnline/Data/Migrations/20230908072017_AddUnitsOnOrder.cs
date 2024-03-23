using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanHangOnline.Data.Migrations
{
    public partial class AddUnitsOnOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "tb_Product");

            migrationBuilder.AddColumn<int>(
                name: "UnitsInStock",
                table: "tb_Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitsOnOrder",
                table: "tb_Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitsInStock",
                table: "tb_Product");

            migrationBuilder.DropColumn(
                name: "UnitsOnOrder",
                table: "tb_Product");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "tb_Product",
                type: "int",
                nullable: true);
        }
    }
}
