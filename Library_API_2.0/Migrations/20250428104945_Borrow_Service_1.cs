using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_API_2._0.Migrations
{
    /// <inheritdoc />
    public partial class Borrow_Service_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BorrowingRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BorrowingDetails",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BorrowingRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingDetails", x => new { x.BookId, x.BorrowingRequestId });
                    table.ForeignKey(
                        name: "FK_BorrowingDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowingDetails_BorrowingRequests_BorrowingRequestId",
                        column: x => x.BorrowingRequestId,
                        principalTable: "BorrowingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingDetails_BorrowingRequestId",
                table: "BorrowingDetails",
                column: "BorrowingRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowingDetails");

            migrationBuilder.DropTable(
                name: "BorrowingRequests");
        }
    }
}
