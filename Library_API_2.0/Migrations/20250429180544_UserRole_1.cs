using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_API_2._0.Migrations
{
    /// <inheritdoc />
    public partial class UserRole_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f", "474b5aff-b40e-4743-a777-04163a8b3215" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f", "474b5aff-b40e-4743-a777-04163a8b3215" });
        }
    }
}
