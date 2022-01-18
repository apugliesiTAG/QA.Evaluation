using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddCustomerAndCorrectionForShippers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "shipper",
                newName: "InteralId");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "shipper",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InteralId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.CustomerId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "shipper",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "InteralId",
                table: "shipper",
                newName: "Value");
        }
    }
}
