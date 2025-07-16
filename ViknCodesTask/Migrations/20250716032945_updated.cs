using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViknCodesTask.Migrations
{
    /// <inheritdoc />
    public partial class updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "VariantOptions");

            migrationBuilder.CreateTable(
                name: "VariantCombinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariantCombinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariantCombinations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubVariantCombinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VariantCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubVariantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubVariantCombinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubVariantCombinations_VariantCombinations_VariantCombinationId",
                        column: x => x.VariantCombinationId,
                        principalTable: "VariantCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubVariantCombinations_VariantOptions_SubVariantId",
                        column: x => x.SubVariantId,
                        principalTable: "VariantOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubVariantCombinations_SubVariantId",
                table: "SubVariantCombinations",
                column: "SubVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_SubVariantCombinations_VariantCombinationId",
                table: "SubVariantCombinations",
                column: "VariantCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_VariantCombinations_ProductId",
                table: "VariantCombinations",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubVariantCombinations");

            migrationBuilder.DropTable(
                name: "VariantCombinations");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "VariantOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
