using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructionProject.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAssignedToFromTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "Tasks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$N4bdM4jG5Gcc9rylWnEoQ.0.zCJYmfJyQoKfxNK/Q28jZLr/47czO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$WXnUBBpFvcjW4PMdTGes8.FL96u4OnvYkBitmW6aDScyRZIGvXxVm");
        }
    }
}
