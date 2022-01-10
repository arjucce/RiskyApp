using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskyApp.Migrations
{
    public partial class updateTableData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Data",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Data",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Data");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Data");
        }
    }
}
