using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Reservations_Fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketType",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "TicketType",
                table: "Passanger",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketType",
                table: "Passanger");

            migrationBuilder.AddColumn<int>(
                name: "TicketType",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
