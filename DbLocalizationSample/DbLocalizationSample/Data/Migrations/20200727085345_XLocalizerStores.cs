using Microsoft.EntityFrameworkCore.Migrations;

namespace DBLocalizationSample.Data.Migrations
{
    public partial class XLocalizerStores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "XDbCultures",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XDbCultures", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "XDbResources",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CultureID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XDbResources", x => x.ID);
                    table.ForeignKey(
                        name: "FK_XDbResources_XDbCultures_CultureID",
                        column: x => x.CultureID,
                        principalTable: "XDbCultures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "XDbCultures",
                columns: new[] { "ID", "EnglishName", "IsActive", "IsDefault" },
                values: new object[] { "en", "English", true, true });

            migrationBuilder.InsertData(
                table: "XDbCultures",
                columns: new[] { "ID", "EnglishName", "IsActive", "IsDefault" },
                values: new object[] { "tr", "Turkish", true, false });

            migrationBuilder.InsertData(
                table: "XDbCultures",
                columns: new[] { "ID", "EnglishName", "IsActive", "IsDefault" },
                values: new object[] { "ar", "Arabic", true, false });

            migrationBuilder.InsertData(
                table: "XDbResources",
                columns: new[] { "ID", "Comment", "CultureID", "IsActive", "Key", "Value" },
                values: new object[] { 1, "Created by XLocalizer", "tr", true, "Welcome", "Hoşgeldin" });

            migrationBuilder.CreateIndex(
                name: "IX_XDbResources_CultureID_Key",
                table: "XDbResources",
                columns: new[] { "CultureID", "Key" },
                unique: true,
                filter: "[CultureID] IS NOT NULL AND [Key] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "XDbResources");

            migrationBuilder.DropTable(
                name: "XDbCultures");
        }
    }
}
