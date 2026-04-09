using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessInfo.Persistence.Migrations
{
    public partial class CreateTableRentalpartner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentalPartner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuerId = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: true),
                    Document = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalPartner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalPartner_Issuer_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "Issuer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentalPartner_IssuerId",
                table: "RentalPartner",
                column: "IssuerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalPartner");
        }
    }
}
