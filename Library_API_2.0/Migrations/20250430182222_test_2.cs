using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_API_2._0.Migrations
{
    /// <inheritdoc />
    public partial class test_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f",
                column: "NormalizedName",
                value: "ADMIN");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "639de03f-7876-4fff-96ec-37f8bd3bf180",
                column: "NormalizedName",
                value: "USER");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3f4a7d2-4f6b-4d7e-8b1a-2c1b6e66d76e",
                column: "NormalizedName",
                value: "VIP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f",
                column: "NormalizedName",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "639de03f-7876-4fff-96ec-37f8bd3bf180",
                column: "NormalizedName",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3f4a7d2-4f6b-4d7e-8b1a-2c1b6e66d76e",
                column: "NormalizedName",
                value: null);
        }
    }
}
