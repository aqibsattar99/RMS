using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEqptissueSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eqptname",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eqptname", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eqptissue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    branchid = table.Column<int>(type: "int", nullable: true),
                    eqptid = table.Column<int>(type: "int", nullable: true),
                    qty = table.Column<int>(type: "int", nullable: true),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true),
            
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eqptissue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eqptissue_Branch_branchid",
                        column: x => x.branchid,
                        principalTable: "Branch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Eqptissue_Eqptname_EqptnameId",
                        column: x => x.eqptid,
                        principalTable: "Eqptname",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Eqptstore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    eqptid = table.Column<int>(type: "int", nullable: true),
                    qty = table.Column<int>(type: "int", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true),
                    EqptnameId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eqptstore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eqptstore_Eqptname_EqptnameId",
                        column: x => x.EqptnameId,
                        principalTable: "Eqptname",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eqptissue_branchid",
                table: "Eqptissue",
                column: "branchid");

            migrationBuilder.CreateIndex(
                name: "IX_Eqptissue_EqptnameId",
                table: "Eqptissue",
                column: "EqptnameId");

            migrationBuilder.CreateIndex(
                name: "IX_Eqptstore_EqptnameId",
                table: "Eqptstore",
                column: "EqptnameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Eqptissue");

            migrationBuilder.DropTable(
                name: "Eqptstore");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Eqptname");
        }
    }
}
