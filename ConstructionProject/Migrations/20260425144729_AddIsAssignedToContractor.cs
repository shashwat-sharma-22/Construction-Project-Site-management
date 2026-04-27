using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructionProject.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAssignedToContractor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAssigned",
                table: "Contractors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$8xz3z5AEMbTveglrUdSs0OniqHJNXI1to1pHVp4WEYXHRYQvd5SV6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAssigned",
                table: "Contractors");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$N4bdM4jG5Gcc9rylWnEoQ.0.zCJYmfJyQoKfxNK/Q28jZLr/47czO");
        }
    }
}
