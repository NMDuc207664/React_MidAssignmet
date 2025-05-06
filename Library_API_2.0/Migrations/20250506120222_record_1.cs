using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library_API_2._0.Migrations
{
    /// <inheritdoc />
    public partial class record_1 : Migration
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
                name: "FK_BorrowingDetails_Books_BookId",
                table: "BorrowingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingDetails_BorrowingRequests_BorrowingRequestId",
                table: "BorrowingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingRequests_AspNetUsers_UserId",
                table: "BorrowingRequests");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "639de03f-7876-4fff-96ec-37f8bd3bf180");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f", "474b5aff-b40e-4743-a777-04163a8b3215" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f");

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BorrowingRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HasPickedUp = table.Column<bool>(type: "bit", nullable: false),
                    PickUpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PickUpAdminId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HasReturned = table.Column<bool>(type: "bit", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReturnAdminId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReturnStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Records_AspNetUsers_PickUpAdminId",
                        column: x => x.PickUpAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Records_AspNetUsers_ReturnAdminId",
                        column: x => x.ReturnAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Records_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Records_BorrowingRequests_BorrowingRequestId",
                        column: x => x.BorrowingRequestId,
                        principalTable: "BorrowingRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_BorrowingRequestId",
                table: "Records",
                column: "BorrowingRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Records_PickUpAdminId",
                table: "Records",
                column: "PickUpAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_ReturnAdminId",
                table: "Records",
                column: "ReturnAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_UserId",
                table: "Records",
                column: "UserId");

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
                name: "FK_BorrowingDetails_Books_BookId",
                table: "BorrowingDetails",
                column: "BookId",
                principalTable: "Books",
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
                name: "FK_BorrowingDetails_Books_BookId",
                table: "BorrowingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingDetails_BorrowingRequests_BorrowingRequestId",
                table: "BorrowingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingRequests_AspNetUsers_UserId",
                table: "BorrowingRequests");

            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f", null, "Admin", "ADMIN" },
                    { "639de03f-7876-4fff-96ec-37f8bd3bf180", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f", "474b5aff-b40e-4743-a777-04163a8b3215" });

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Books_BookId",
                table: "BookGenres",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Genres_GenreId",
                table: "BookGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowingDetails_Books_BookId",
                table: "BorrowingDetails",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
        }
    }
}
