using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_API_2._0.Migrations
{
    /// <inheritdoc />
    public partial class record_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_Authors_AuthorId",
                table: "BookAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_Books_BookId",
                table: "BookAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Books_BookId",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Genres_GenreId",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingDetails_BorrowingRequests_BorrowingRequestId",
                table: "BorrowingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingRequests_AspNetUsers_UserId",
                table: "BorrowingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_AspNetUsers_ReturnAdminId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_AspNetUsers_UserId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_BorrowingRequests_BorrowingRequestId",
                table: "Records");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_Authors_AuthorId",
                table: "BookAuthors",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_Books_BookId",
                table: "BookAuthors",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Books_BookId",
                table: "BookGenres",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Genres_GenreId",
                table: "BookGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowingDetails_BorrowingRequests_BorrowingRequestId",
                table: "BorrowingDetails",
                column: "BorrowingRequestId",
                principalTable: "BorrowingRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowingRequests_AspNetUsers_UserId",
                table: "BorrowingRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_AspNetUsers_ReturnAdminId",
                table: "Records",
                column: "ReturnAdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_AspNetUsers_UserId",
                table: "Records",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_BorrowingRequests_BorrowingRequestId",
                table: "Records",
                column: "BorrowingRequestId",
                principalTable: "BorrowingRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_Authors_AuthorId",
                table: "BookAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_Books_BookId",
                table: "BookAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Books_BookId",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Genres_GenreId",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingDetails_BorrowingRequests_BorrowingRequestId",
                table: "BorrowingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingRequests_AspNetUsers_UserId",
                table: "BorrowingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_AspNetUsers_ReturnAdminId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_AspNetUsers_UserId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_BorrowingRequests_BorrowingRequestId",
                table: "Records");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_Authors_AuthorId",
                table: "BookAuthors",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_Books_BookId",
                table: "BookAuthors",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Books_BookId",
                table: "BookGenres",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Genres_GenreId",
                table: "BookGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowingDetails_BorrowingRequests_BorrowingRequestId",
                table: "BorrowingDetails",
                column: "BorrowingRequestId",
                principalTable: "BorrowingRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowingRequests_AspNetUsers_UserId",
                table: "BorrowingRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_AspNetUsers_ReturnAdminId",
                table: "Records",
                column: "ReturnAdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_AspNetUsers_UserId",
                table: "Records",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_BorrowingRequests_BorrowingRequestId",
                table: "Records",
                column: "BorrowingRequestId",
                principalTable: "BorrowingRequests",
                principalColumn: "Id");
        }
    }
}
