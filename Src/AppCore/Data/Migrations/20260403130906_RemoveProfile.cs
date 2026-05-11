using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AppUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "AppUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AppUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "AppUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AppUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "AppUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "AppUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportNumber",
                table: "AppUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "AppUsers");

            migrationBuilder.CreateTable(
                name: "AppProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppProfiles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppProfiles_UserId",
                table: "AppProfiles",
                column: "UserId",
                unique: true);
        }
    }
}
