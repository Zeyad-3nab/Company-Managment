using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.Test.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfCreation",
                table: "employees",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GetDate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "WorkforId",
                table: "employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_WorkforId",
                table: "employees",
                column: "WorkforId");

            migrationBuilder.AddForeignKey(
                name: "FK_employees_Departments_WorkforId",
                table: "employees",
                column: "WorkforId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employees_Departments_WorkforId",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_WorkforId",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "WorkforId",
                table: "employees");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfCreation",
                table: "employees",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GetDate()");
        }
    }
}
