using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetTask.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddManagerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "ManagerName",
                table: "Employees",
                newName: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerId",
                table: "Employees",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_ManagerId",
                table: "Employees",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_ManagerId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ManagerId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Employees",
                newName: "ManagerName");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tickets",
                type: "TEXT",
                nullable: true);
        }
    }
}
