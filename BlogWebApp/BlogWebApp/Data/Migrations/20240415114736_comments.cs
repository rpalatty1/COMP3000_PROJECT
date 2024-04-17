using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class comments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Category_CategoryId",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_Blog_SubCategory_SubCategoryId",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogImages_SubCategory_SubCategoryId",
                table: "BlogImages");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategory_Category_CategoryId",
                table: "SubCategory");

            migrationBuilder.CreateTable(
                name: "MainComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogId = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainComment_Blog_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blog",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainCommentId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubComment_MainComment_MainCommentId",
                        column: x => x.MainCommentId,
                        principalTable: "MainComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainComment_BlogId",
                table: "MainComment",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_SubComment_MainCommentId",
                table: "SubComment",
                column: "MainCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Category_CategoryId",
                table: "Blog",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_SubCategory_SubCategoryId",
                table: "Blog",
                column: "SubCategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogImages_SubCategory_SubCategoryId",
                table: "BlogImages",
                column: "SubCategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategory_Category_CategoryId",
                table: "SubCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Category_CategoryId",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_Blog_SubCategory_SubCategoryId",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogImages_SubCategory_SubCategoryId",
                table: "BlogImages");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategory_Category_CategoryId",
                table: "SubCategory");

            migrationBuilder.DropTable(
                name: "SubComment");

            migrationBuilder.DropTable(
                name: "MainComment");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Category_CategoryId",
                table: "Blog",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_SubCategory_SubCategoryId",
                table: "Blog",
                column: "SubCategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogImages_SubCategory_SubCategoryId",
                table: "BlogImages",
                column: "SubCategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategory_Category_CategoryId",
                table: "SubCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }
    }
}
