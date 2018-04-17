using Microsoft.EntityFrameworkCore.Migrations;

namespace AprioritWebCalendar.Data.Migrations
{
    public partial class Add_Event_IsAllDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAllDay",
                table: "Events",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAllDay",
                table: "Events");
        }
    }
}
