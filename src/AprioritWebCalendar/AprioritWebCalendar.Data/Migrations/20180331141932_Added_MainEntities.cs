using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AprioritWebCalendar.Data.Migrations
{
    public partial class Added_MainEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeOffset",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Color = table.Column<string>(maxLength: 7, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    OwnerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calendars_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<TimeSpan>(nullable: false),
                    IsPrivate = table.Column<bool>(nullable: false),
                    Lattitude = table.Column<double>(nullable: true),
                    LocationDescription = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OwnerId = table.Column<int>(nullable: false),
                    RemindBefore = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    StartTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCalendars",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    CalendarId = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    IsSubscribed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCalendars", x => new { x.UserId, x.CalendarId });
                    table.ForeignKey(
                        name: "FK_UserCalendars_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCalendars_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EventCalendars",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    CalendarId = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCalendars", x => new { x.EventId, x.CalendarId });
                    table.ForeignKey(
                        name: "FK_EventCalendars_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventCalendars_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EventPeriods",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    CycleDays = table.Column<int>(nullable: true),
                    PeriodEnd = table.Column<DateTime>(nullable: false),
                    PeriodStart = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    WholeDaysCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPeriods", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_EventPeriods_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    InvitatorId = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => new { x.EventId, x.UserId, x.InvitatorId });
                    table.UniqueConstraint("AK_Invitations_EventId_InvitatorId_UserId", x => new { x.EventId, x.InvitatorId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Invitations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invitations_AspNetUsers_InvitatorId",
                        column: x => x.InvitatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Invitations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_OwnerId",
                table: "Calendars",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_EventCalendars_CalendarId",
                table: "EventCalendars",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OwnerId",
                table: "Events",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InvitatorId",
                table: "Invitations",
                column: "InvitatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_UserId",
                table: "Invitations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCalendars_CalendarId",
                table: "UserCalendars",
                column: "CalendarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventCalendars");

            migrationBuilder.DropTable(
                name: "EventPeriods");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "UserCalendars");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropColumn(
                name: "TimeOffset",
                table: "AspNetUsers");
        }
    }
}
