using System;
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

            migrationBuilder.AddColumn<int>(
                name: "TelegramId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TelegramCodes",
                columns: table => new
                {
                    TelegramId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramCodes", x => new { x.TelegramId, x.Code });
                });
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
