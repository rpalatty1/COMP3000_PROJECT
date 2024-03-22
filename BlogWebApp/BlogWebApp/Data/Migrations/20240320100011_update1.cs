using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Blog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.CreateTable(
            //        name: "Blog",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //            CategoryId = table.Column<int>(type: "int", nullable: false),
            //            SubCategoryId = table.Column<int>(type: "int", nullable: false),
            //            Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //            CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //            Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //            IsActive = table.Column<bool>(type: "bit", nullable: false),
            //            ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //            Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //            Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //            Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //            TitleImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_Blog", x => x.Id);
            //            table.ForeignKey(
            //                name: "FK_Blog_AspNetUsers_ApplicationUserId",
            //                column: x => x.ApplicationUserId,
            //                principalTable: "AspNetUsers",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //            table.ForeignKey(
            //                name: "FK_Blog_Category_CategoryId",
            //                column: x => x.CategoryId,
            //                principalTable: "Category",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //            table.ForeignKey(
            //                name: "FK_Blog_SubCategory_SubCategoryId",
            //                column: x => x.SubCategoryId,
            //                principalTable: "SubCategory",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //        });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Blog_ApplicationUserId",
            //        table: "Blog",
            //        column: "ApplicationUserId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Blog_CategoryId",
            //        table: "Blog",
            //        column: "CategoryId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Blog_SubCategoryId",
            //        table: "Blog",
            //        column: "SubCategoryId");
        }
    }
}
