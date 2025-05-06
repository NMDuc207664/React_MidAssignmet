using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_API_2._0.Migrations
{
    /// <inheritdoc />
    public partial class genreUpdate_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3f4a7d2-4f6b-4d7e-8b1a-2c1b6e66d76e");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c3f4a7d2-4f6b-4d7e-8b1a-2c1b6e66d76e", null, "Vip", "VIP" });
        }
    }
}
