using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RateLimitValidator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValidationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(200)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationRequests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ValidationRequest_PhoneNumber_Time_Ascending",
                table: "ValidationRequests",
                columns: new[] { "PhoneNumber", "Time" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValidationRequests");
        }
    }
}
