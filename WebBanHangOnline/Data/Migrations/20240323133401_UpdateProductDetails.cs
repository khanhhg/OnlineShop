using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanHangOnline.Data.Migrations
{
    public partial class UpdateProductDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_OrderDetail",
                table: "tb_OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_tb_OrderDetail_OrderId",
                table: "tb_OrderDetail");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "tb_ProductCategory");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "tb_ProductCategory");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "tb_ProductCategory");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "tb_Product");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "tb_Product");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "tb_Product");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "tb_OrderDetail");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "tb_News");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "tb_News");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "tb_News");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "tb_Category");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "tb_Category");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "tb_Category");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_OrderDetail",
                table: "tb_OrderDetail",
                columns: new[] { "OrderId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_OrderDetail",
                table: "tb_OrderDetail");

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "tb_ProductCategory",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "tb_ProductCategory",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "tb_ProductCategory",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "tb_Product",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "tb_Product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "tb_Product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                table: "tb_OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "tb_News",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "tb_News",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "tb_News",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "tb_Category",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "tb_Category",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "tb_Category",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_OrderDetail",
                table: "tb_OrderDetail",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_OrderDetail_OrderId",
                table: "tb_OrderDetail",
                column: "OrderId");
        }
    }
}
