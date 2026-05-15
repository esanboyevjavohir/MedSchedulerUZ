using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedSchedulerUZ.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class added_seed_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Hospitals_HospitalId",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "HospitalId",
                table: "Users",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordTokenExpiry",
                table: "Users",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "ShiftSwaps",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedOn", "DepartmentId", "Email", "FullName", "HospitalId", "IsActive", "PasswordHash", "PhoneNumber", "RefreshToken", "RefreshTokenExpireDate", "ResetPasswordToken", "ResetPasswordTokenExpiry", "RoleType", "Salt", "SpecializationId", "UpdatedOn" },
                values: new object[] { new Guid("a0ae7f44-f3a2-4ea6-8030-01a4ea1b1ae3"), new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "javohiresanboyev053@gmail.com", "Esanboyev Javohir", null, true, "q7Mgq0zaGD0sAPTnwO8j9d69t78KJb5vBJ3VRNq5lfQ=", "+998933116612", null, null, null, null, 1, "f67273d6-d1ee-4129-9740-75a8df1a5c5b", null, null });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Hospitals_HospitalId",
                table: "Users",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Hospitals_HospitalId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a0ae7f44-f3a2-4ea6-8030-01a4ea1b1ae3"));

            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetPasswordTokenExpiry",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "ShiftSwaps");

            migrationBuilder.AlterColumn<Guid>(
                name: "HospitalId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Hospitals_HospitalId",
                table: "Users",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
