using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskyApp.Migrations
{
    public partial class updateDepartmentOperation_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "DepartmentOperations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "DepartmentOperations");
        }
    }
}
