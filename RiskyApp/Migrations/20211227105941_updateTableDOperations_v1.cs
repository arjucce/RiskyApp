using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskyApp.Migrations
{
    public partial class updateTableDOperations_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormatUrl",
                table: "DepartmentOperations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormatUrl",
                table: "DepartmentOperations");
        }
    }
}
