using Microsoft.EntityFrameworkCore.Migrations;

namespace AprioritWebCalendar.Data.Migrations
{
    public partial class Added_Telegram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTelegramNotificationEnabled",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelegramId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TelegramCodes",
                columns: table => new
                {
                    TelegramId = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramCodes", x => x.TelegramId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelegramCodes_Code",
                table: "TelegramCodes",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelegramCodes");

            migrationBuilder.DropColumn(
                name: "IsTelegramNotificationEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TelegramId",
                table: "AspNetUsers");
        }
    }
}
