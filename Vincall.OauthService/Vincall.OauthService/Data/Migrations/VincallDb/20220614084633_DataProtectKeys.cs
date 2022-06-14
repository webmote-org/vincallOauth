using Microsoft.EntityFrameworkCore.Migrations;

namespace Vincall.OauthService.Data.Migrations.VincallDb
{
    public partial class DataProtectKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FriendlyName = table.Column<string>(maxLength: 50000, nullable: true),
                    Xml = table.Column<string>(maxLength: 50000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
          name: "DataProtectionKeys");
        }
    }
}
