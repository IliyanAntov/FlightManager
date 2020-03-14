using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class PlaneNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PlaneNumber",
                table: "Flights",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PlaneNumber",
                table: "Flights",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
