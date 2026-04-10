using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library.Web.Migrations
{
    /// <inheritdoc />
    public partial class newMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "0d4e27db-c6b4-4b22-b17e-8dc6c81cb101", "Admin", "ADMIN" },
                    { 2, "0d4e27db-c6b4-4b22-b17e-8dc6c81cb102", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98001", "alice.johnson@library.com", true, "Alice Johnson", false, null, "ALICE.JOHNSON@LIBRARY.COM", "ALICE.JOHNSON", null, "01000000001", true, "f8e2a9a2-c93f-4dbf-a0f9-13f4739db001", false, "alice.johnson" },
                    { 2, 0, "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98002", "bob.smith@library.com", true, "Bob Smith", false, null, "BOB.SMITH@LIBRARY.COM", "BOB.SMITH", null, "01000000002", true, "f8e2a9a2-c93f-4dbf-a0f9-13f4739db002", false, "bob.smith" },
                    { 3, 0, "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98003", "carla.davis@library.com", true, "Carla Davis", false, null, "CARLA.DAVIS@LIBRARY.COM", "CARLA.DAVIS", null, "01000000003", true, "f8e2a9a2-c93f-4dbf-a0f9-13f4739db003", false, "carla.davis" },
                    { 4, 0, "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98004", "david.brown@library.com", true, "David Brown", false, null, "DAVID.BROWN@LIBRARY.COM", "DAVID.BROWN", null, "01000000004", true, "f8e2a9a2-c93f-4dbf-a0f9-13f4739db004", false, "david.brown" },
                    { 5, 0, "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98005", "eva.wilson@library.com", true, "Eva Wilson", false, null, "EVA.WILSON@LIBRARY.COM", "EVA.WILSON", null, "01000000005", true, "f8e2a9a2-c93f-4dbf-a0f9-13f4739db005", false, "eva.wilson" },
                    { 6, 0, "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98006", "frank.miller@library.com", true, "Frank Miller", false, null, "FRANK.MILLER@LIBRARY.COM", "FRANK.MILLER", null, "01000000006", true, "f8e2a9a2-c93f-4dbf-a0f9-13f4739db006", false, "frank.miller" },
                    { 7, 0, "7e9b7c8e-f8f5-4ec7-88e1-01fc8cb98007", "grace.lee@library.com", true, "Grace Lee", false, null, "GRACE.LEE@LIBRARY.COM", "GRACE.LEE", null, "01000000007", true, "f8e2a9a2-c93f-4dbf-a0f9-13f4739db007", false, "grace.lee" }
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Bio", "Name" },
                values: new object[,]
                {
                    { 1, "Award-winning fiction writer.", "Nora Allen" },
                    { 2, "Researcher in applied sciences.", "Samir Hassan" },
                    { 3, "Historian focused on modern history.", "Linda Carter" },
                    { 4, "Software architect and technical author.", "Omar Farouk" },
                    { 5, "Philosophy lecturer and essayist.", "Hana Youssef" },
                    { 6, "Biographer of notable leaders.", "James Walker" },
                    { 7, "Art critic and curator.", "Mia Chen" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fiction" },
                    { 2, "Science" },
                    { 3, "History" },
                    { 4, "Technology" },
                    { 5, "Philosophy" },
                    { 6, "Biography" },
                    { 7, "Art" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 },
                    { 2, 6 },
                    { 2, 7 }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "CategoryId", "Description", "Img", "Price", "Title" },
                values: new object[,]
                {
                    { 1, 1, "A contemporary fiction novel.", "images/books/silent-path.jpg", 120.00m, "The Silent Path" },
                    { 2, 2, "Practical science concepts for everyone.", "images/books/physics-daily-life.jpg", 150.00m, "Physics in Daily Life" },
                    { 3, 3, "A journey through important historical events.", "images/books/echoes-past.jpg", 135.00m, "Echoes of the Past" },
                    { 4, 4, "Guide to building scalable web apps.", "images/books/web-engineering.jpg", 220.00m, "Modern Web Engineering" },
                    { 5, 5, "An introduction to core philosophy topics.", "images/books/questions-meaning.jpg", 140.00m, "Questions of Meaning" },
                    { 6, 6, "Biography of an influential innovator.", "images/books/life-pioneer.jpg", 160.00m, "Life of a Pioneer" },
                    { 7, 7, "Understanding art movements and styles.", "images/books/seeing-colors.jpg", 145.00m, "Seeing Through Colors" }
                });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "Amount", "ApplicationUserId", "DueAt", "RentedAt", "ReturnedAt", "Status" },
                values: new object[,]
                {
                    { 1, 25.00m, 1, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 30.00m, 2, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0 },
                    { 3, 35.00m, 3, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2 },
                    { 4, 28.00m, 4, new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 5, 26.00m, 5, new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0 },
                    { 6, 32.00m, 6, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 7, 29.00m, 7, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0 }
                });

            migrationBuilder.InsertData(
                table: "BookAuthors",
                columns: new[] { "AuthorId", "BookId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 5 },
                    { 6, 6 },
                    { 7, 7 }
                });

            migrationBuilder.InsertData(
                table: "Copies",
                columns: new[] { "Id", "AllowToRental", "BookId", "Name" },
                values: new object[,]
                {
                    { 1, true, 1, "Copy-001" },
                    { 2, true, 1, "Copy-002" },
                    { 3, true, 1, "Copy-003" },
                    { 4, true, 1, "Copy-004" },
                    { 5, true, 2, "Copy-005" },
                    { 6, true, 2, "Copy-006" },
                    { 7, true, 2, "Copy-007" },
                    { 8, true, 2, "Copy-008" },
                    { 9, true, 3, "Copy-009" },
                    { 10, true, 3, "Copy-010" },
                    { 11, true, 3, "Copy-011" },
                    { 12, true, 3, "Copy-012" },
                    { 13, true, 4, "Copy-013" },
                    { 14, true, 4, "Copy-014" },
                    { 15, true, 4, "Copy-015" },
                    { 16, true, 4, "Copy-016" },
                    { 17, true, 5, "Copy-017" },
                    { 18, true, 5, "Copy-018" },
                    { 19, true, 5, "Copy-019" },
                    { 20, true, 5, "Copy-020" },
                    { 21, true, 6, "Copy-021" },
                    { 22, true, 6, "Copy-022" },
                    { 23, true, 6, "Copy-023" },
                    { 24, true, 6, "Copy-024" },
                    { 25, true, 7, "Copy-025" },
                    { 26, true, 7, "Copy-026" },
                    { 27, true, 7, "Copy-027" },
                    { 28, true, 7, "Copy-028" }
                });

            migrationBuilder.InsertData(
                table: "CopyRentals",
                columns: new[] { "CopyId", "RentalId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 5 },
                    { 6, 6 },
                    { 7, 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 7 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 7, 7 });

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "CopyRentals",
                keyColumns: new[] { "CopyId", "RentalId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "CopyRentals",
                keyColumns: new[] { "CopyId", "RentalId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "CopyRentals",
                keyColumns: new[] { "CopyId", "RentalId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "CopyRentals",
                keyColumns: new[] { "CopyId", "RentalId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "CopyRentals",
                keyColumns: new[] { "CopyId", "RentalId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "CopyRentals",
                keyColumns: new[] { "CopyId", "RentalId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "CopyRentals",
                keyColumns: new[] { "CopyId", "RentalId" },
                keyValues: new object[] { 7, 7 });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Copies",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
