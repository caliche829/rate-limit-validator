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
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationRequests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ValidationRequest_PhoneNumber_Time_Ascending",
                table: "ValidationRequests",
                columns: new[] { "PhoneNumber", "Time" });

            migrationBuilder.Sql(@"exec('create view dbo.ValidationRequestReport as 
SELECT
    PhoneNumber
    ,CAST(FORMAT([Time],''yyyy-MM-dd HH:mm:ss'') AS datetime) AS [Time]
    , SUM(IIF(IsSuccess = 1, 1, 0)) TotalSuccess
    , SUM(IIF(IsSuccess = 0, 1, 0)) TotalError
FROM [dbo].[ValidationRequests]
GROUP BY 
    PhoneNumber
    ,CAST(FORMAT([Time],''yyyy-MM-dd HH:mm:ss'') AS datetime)
');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValidationRequests");

            migrationBuilder.Sql("exec('drop view dbo.ValidationRequestReport');");
        }
    }
}
