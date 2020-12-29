using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wasserwacht.DigitalGuardBook.Common.Data.Migrations
{
    public partial class CreateInitialDataStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "common");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "common");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "common");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "common");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "common");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "common");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "common");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "common");

            migrationBuilder.CreateTable(
                name: "Organisations",
                schema: "common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganisationPerson",
                schema: "common",
                columns: table => new
                {
                    MangedOrganisationsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TechnicalLeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationPerson", x => new { x.MangedOrganisationsId, x.TechnicalLeadId });
                    table.ForeignKey(
                        name: "FK_OrganisationPerson_AspNetUsers_TechnicalLeadId",
                        column: x => x.TechnicalLeadId,
                        principalSchema: "common",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganisationPerson_Organisations_MangedOrganisationsId",
                        column: x => x.MangedOrganisationsId,
                        principalSchema: "common",
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganisationPerson1",
                schema: "common",
                columns: table => new
                {
                    MembersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganisationsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationPerson1", x => new { x.MembersId, x.OrganisationsId });
                    table.ForeignKey(
                        name: "FK_OrganisationPerson1_AspNetUsers_MembersId",
                        column: x => x.MembersId,
                        principalSchema: "common",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganisationPerson1_Organisations_OrganisationsId",
                        column: x => x.OrganisationsId,
                        principalSchema: "common",
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                schema: "common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stations_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "common",
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogBooks",
                schema: "common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    StationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogBooks_Stations_StationId",
                        column: x => x.StationId,
                        principalSchema: "common",
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogBookEntry",
                schema: "common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeStamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LogBookId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogBookEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogBookEntry_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "common",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogBookEntry_LogBooks_LogBookId",
                        column: x => x.LogBookId,
                        principalSchema: "common",
                        principalTable: "LogBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogBookEntry_AuthorId",
                schema: "common",
                table: "LogBookEntry",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_LogBookEntry_LogBookId",
                schema: "common",
                table: "LogBookEntry",
                column: "LogBookId");

            migrationBuilder.CreateIndex(
                name: "IX_LogBooks_StationId",
                schema: "common",
                table: "LogBooks",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationPerson_TechnicalLeadId",
                schema: "common",
                table: "OrganisationPerson",
                column: "TechnicalLeadId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationPerson1_OrganisationsId",
                schema: "common",
                table: "OrganisationPerson1",
                column: "OrganisationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_OrganisationId",
                schema: "common",
                table: "Stations",
                column: "OrganisationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogBookEntry",
                schema: "common");

            migrationBuilder.DropTable(
                name: "OrganisationPerson",
                schema: "common");

            migrationBuilder.DropTable(
                name: "OrganisationPerson1",
                schema: "common");

            migrationBuilder.DropTable(
                name: "LogBooks",
                schema: "common");

            migrationBuilder.DropTable(
                name: "Stations",
                schema: "common");

            migrationBuilder.DropTable(
                name: "Organisations",
                schema: "common");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "common",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "common",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "common",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "common",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "common",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "common",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "common",
                newName: "AspNetRoleClaims");
        }
    }
}
