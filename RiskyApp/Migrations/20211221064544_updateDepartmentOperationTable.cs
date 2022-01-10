using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskyApp.Migrations
{
    public partial class updateDepartmentOperationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "DepartmentOperations");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "DepartmentOperations");

            migrationBuilder.AddColumn<int>(
                name: "OperationTypeId",
                table: "DepartmentOperations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationTypeId",
                table: "DepartmentOperations");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "DepartmentOperations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "DepartmentOperations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
