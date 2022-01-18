using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class CorrectOrderID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "order",
                newName: "OrderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "order",
                newName: "OrderId");
        }
    }
}
