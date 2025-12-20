using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bsmtHackathon.Migrations
{
    /// <inheritdoc />
    public partial class IlkKurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OgrenciProfilleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    BaslangicButcesi = table.Column<decimal>(type: "TEXT", nullable: false),
                    KalanButce = table.Column<decimal>(type: "TEXT", nullable: false),
                    Sehir = table.Column<string>(type: "TEXT", nullable: true),
                    MutfakEkipmanlari = table.Column<string>(type: "TEXT", nullable: true),
                    Alerjiler = table.Column<string>(type: "TEXT", nullable: true),
                    BeslenmeTercihi = table.Column<string>(type: "TEXT", nullable: true),
                    EkstraNotlar = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OgrenciProfilleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdSoyad = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YemekPlanlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlanJsonData = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YemekPlanlari", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OgrenciProfilleri");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "YemekPlanlari");
        }
    }
}
