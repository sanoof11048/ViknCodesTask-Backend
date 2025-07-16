using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViknCodesTask.Migrations
{
    /// <inheritdoc />
    public partial class updated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubVariantCombinations");

            migrationBuilder.DropTable(
                name: "VariantCombinations");

            migrationBuilder.DropIndex(
                name: "IX_VariantOptions_ProductVariantId",
                table: "VariantOptions");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "VariantOptions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ProductStocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VariantKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VariantOptions_ProductVariantId_Value",
                table: "VariantOptions",
                columns: new[] { "ProductVariantId", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId_VariantName",
                table: "ProductVariants",
                columns: new[] { "ProductId", "VariantName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCode",
                table: "Products",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductStocks_ProductId_VariantKey",
                table: "ProductStocks",
                columns: new[] { "ProductId", "VariantKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductStocks");

            migrationBuilder.DropIndex(
                name: "IX_VariantOptions_ProductVariantId_Value",
                table: "VariantOptions");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_ProductId_VariantName",
                table: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductCode",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "VariantOptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
                    SubVariantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VariantCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "IX_VariantOptions_ProductVariantId",
                table: "VariantOptions",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

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
    }
}
