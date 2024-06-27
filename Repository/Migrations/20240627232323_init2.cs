using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeartRate",
                schema: "Identity",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "OxygenRate",
                schema: "Identity",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "Identity",
                table: "Patients",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Identity",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FName",
                schema: "Identity",
                table: "Patients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LName",
                schema: "Identity",
                table: "Patients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Identity",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FName",
                schema: "Identity",
                table: "Doctors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LName",
                schema: "Identity",
                table: "Doctors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                schema: "Identity",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "Identity",
                table: "Doctors",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "AlarmMessage",
                schema: "Identity",
                table: "Alarms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "SensorData",
                schema: "Identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeartRate = table.Column<int>(type: "int", nullable: false),
                    OxygenRate = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorData", x => x.id);
                    table.ForeignKey(
                        name: "FK_SensorData_Patients_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "Identity",
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "UserName" },
                values: new object[] { "cbc6195b-4a08-4bfd-b371-a4d495e9f1d5", "Admin@example.com", "Admin", "Admin", "AQAAAAIAAYagAAAAEAaHKou+8fp4jRoxVm84hOB26P00N++LRvmTHEBK7Q24XD4jxU/z/uZXchB/aI1U1w==", "012152001", true, "5c7fac49-70a9-404b-b574-35093214a20a", "Admin@example.com" });

            migrationBuilder.CreateIndex(
                name: "IX_SensorData_PatientId",
                schema: "Identity",
                table: "SensorData",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorData",
                schema: "Identity");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Identity",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "FName",
                schema: "Identity",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "LName",
                schema: "Identity",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Identity",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "FName",
                schema: "Identity",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "LName",
                schema: "Identity",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Password",
                schema: "Identity",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "Identity",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "UserName",
                schema: "Identity",
                table: "Patients",
                newName: "Name");

            migrationBuilder.AddColumn<decimal>(
                name: "HeartRate",
                schema: "Identity",
                table: "Patients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OxygenRate",
                schema: "Identity",
                table: "Patients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmMessage",
                schema: "Identity",
                table: "Alarms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "UserName" },
                values: new object[] { "eaa9b979-0d2c-4e39-bb60-f98a772b5f74", "admin@example.com", null, null, "1254515156", null, false, "", "admin@example.com" });
        }
    }
}
