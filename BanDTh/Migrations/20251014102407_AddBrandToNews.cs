using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanDTh.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandToNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "News",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_BrandId",
                table: "News",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Brands_BrandId",
                table: "News",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Brands_BrandId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_BrandId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "News");
        }
    }
}
