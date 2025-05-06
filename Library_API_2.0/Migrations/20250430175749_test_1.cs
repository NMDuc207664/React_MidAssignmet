using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_API_2._0.Migrations
{
    /// <inheritdoc />
    public partial class test_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingDetails_Books_BookId",
                table: "BorrowingDetails");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BorrowingDetails");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BorrowingRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c3f4a7d2-4f6b-4d7e-8b1a-2c1b6e66d76e", null, "Vip", null });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingRequests_UserId",
                table: "BorrowingRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowingDetails_Books_BookId",
                table: "BorrowingDetails",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowingRequests_AspNetUsers_UserId",
                table: "BorrowingRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingDetails_Books_BookId",
                table: "BorrowingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingRequests_AspNetUsers_UserId",
                table: "BorrowingRequests");

            migrationBuilder.DropIndex(
                name: "IX_BorrowingRequests_UserId",
                table: "BorrowingRequests");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3f4a7d2-4f6b-4d7e-8b1a-2c1b6e66d76e");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BorrowingRequests");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "BorrowingDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowingDetails_Books_BookId",
                table: "BorrowingDetails",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
