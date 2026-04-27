using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructionProject.Migrations
{
    /// <inheritdoc />
    public partial class AddContractorToProjectAndWorkerToTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractorId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$WXnUBBpFvcjW4PMdTGes8.FL96u4OnvYkBitmW6aDScyRZIGvXxVm");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_WorkerId",
                table: "Tasks",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ContractorId",
                table: "Projects",
                column: "ContractorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Contractors_ContractorId",
                table: "Projects",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "ContractorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Workforces_WorkerId",
                table: "Tasks",
                column: "WorkerId",
                principalTable: "Workforces",
                principalColumn: "WorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Contractors_ContractorId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Workforces_WorkerId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_WorkerId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ContractorId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ContractorId",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$/iSU4yE5Jgs3hwacfeqPY.Qv2Ob72jpZld0J8x.SlH0RVJ1zfnMfO");
        }
    }
}
