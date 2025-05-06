using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library_API_2._0.Migrations
{
    /// <inheritdoc />
    public partial class Roles_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
