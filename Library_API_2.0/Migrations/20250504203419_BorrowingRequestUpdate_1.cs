using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_API_2._0.Migrations
{
    /// <inheritdoc />
    public partial class BorrowingRequestUpdate_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedorDeniedDate",
                table: "BorrowingRequests",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedorDeniedDate",
                table: "BorrowingRequests");
        }
    }
}
