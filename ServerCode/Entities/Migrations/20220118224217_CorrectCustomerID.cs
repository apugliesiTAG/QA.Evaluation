using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class CorrectCustomerID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "order",
                newName: "CustomerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "order",
                newName: "CustomerId");
        }
    }
}
