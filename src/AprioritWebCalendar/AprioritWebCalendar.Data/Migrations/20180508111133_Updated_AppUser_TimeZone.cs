using Microsoft.EntityFrameworkCore.Migrations;

namespace AprioritWebCalendar.Data.Migrations
{
    public partial class Updated_AppUser_TimeZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOffset",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "TimeOffset",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
