using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dbs2webapp.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AccountRole__3214EC0757F8FBB8", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Course_category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Course_c__3214EC0711D06CA9", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Question__3214EC07BE6F4E22", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "char(60)", unicode: false, fixedLength: true, maxLength: 60, nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AppUser__3214EC07C0C0ECF6", x => x.Id);
                    table.ForeignKey(
                        name: "FK__AppUser__RoleId__3A81B327",
                        column: x => x.RoleId,
                        principalTable: "AccountRole",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Course_categoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Course__3214EC07ADE10529", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Course__Course_c__3F466844",
                        column: x => x.Course_categoryId,
                        principalTable: "Course_category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Option__3214EC0720FD934B", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Option__Question__5812160E",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Contents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Chapter__3214EC0715667D5C", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Chapter__CourseI__45F365D3",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "User_Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User_Cou__3214EC07D1836DD5", x => x.Id);
                    table.ForeignKey(
                        name: "FK__User_Cour__Cours__4316F928",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__User_Cour__UserI__4222D4EF",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    ChapterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Assignme__3214EC072522C8E5", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Assignmen__Chapt__5BE2A6F2",
                        column: x => x.ChapterId,
                        principalTable: "Chapter",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Assignmen__UserI__5AEE82B9",
                        column: x => x.TeacherId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Chapter_content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TextFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChapterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Chapter___3214EC07967DB6BE", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Chapter_c__Chapt__48CFD27E",
                        column: x => x.ChapterId,
                        principalTable: "Chapter",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxAttempts = table.Column<int>(type: "int", nullable: false),
                    ChapterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Test__3214EC077FA06AA2", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Test__ChapterId__4BAC3F29",
                        column: x => x.ChapterId,
                        principalTable: "Chapter",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Assignment_submission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Assignme__3214EC079993DCD9", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Assignmen__Assig__5EBF139D",
                        column: x => x.AssignmentId,
                        principalTable: "Assignment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Assignmen__UserI__5FB337D6",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Assignment_User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignment_User",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Assignment_User_Assignment",
                        column: x => x.AssignmentId,
                        principalTable: "Assignment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Test_instance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Attempt = table.Column<int>(type: "int", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Test_ins__3214EC07C1C5D6D4", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Test_inst__TestI__4E88ABD4",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Test_inst__UserI__4F7CD00D",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Test_Question",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Test_Que__3214EC076A260717", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Test_Ques__Quest__5441852A",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Test_Ques__TestI__5535A963",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Student" },
                    { 2, "Učitel" },
                    { 3, "Administrátor" }
                });

            migrationBuilder.InsertData(
                table: "Course_category",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Programování" },
                    { 2, "Databáze" },
                    { 3, "Kybernetická bezpečnost" }
                });

            migrationBuilder.InsertData(
                table: "Question",
                columns: new[] { "Id", "Content" },
                values: new object[,]
                {
                    { 1, "Co znamená SQL?" },
                    { 2, "Který příkaz získává všechna data z tabulky?" }
                });

            migrationBuilder.InsertData(
                table: "AppUser",
                columns: new[] { "Id", "Email", "Firstname", "Password", "RoleId", "Surname" },
                values: new object[,]
                {
                    { 1, "student1@example.com", "Jan", "hashed_password1", 1, "Novák" },
                    { 2, "ucitel1@example.com", "Petr", "hashed_password2", 2, "Dvořák" },
                    { 3, "admin1@example.com", "Karel", "hashed_password3", 3, "Svoboda" }
                });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "Course_categoryId", "Name" },
                values: new object[,]
                {
                    { 1, 2, "SQL pro začátečníky" },
                    { 2, 1, "C# pokročilé techniky" },
                    { 3, 3, "Základy kybernetické bezpečnosti" }
                });

            migrationBuilder.InsertData(
                table: "Option",
                columns: new[] { "Id", "IsCorrect", "QuestionId", "Text" },
                values: new object[,]
                {
                    { 1, true, 1, "Structured Query Language" },
                    { 2, false, 1, "System Query List" },
                    { 3, true, 2, "SELECT * FROM tabulka;" },
                    { 4, false, 2, "GET ALL FROM tabulka;" }
                });

            migrationBuilder.InsertData(
                table: "Chapter",
                columns: new[] { "Id", "Contents", "CourseId", "Name" },
                values: new object[,]
                {
                    { 1, "Tato kapitola se věnuje základům SQL.", 1, "Úvod do SQL" },
                    { 2, "Jak správně psát SELECT dotazy.", 1, "SELECT příkazy" },
                    { 3, "Začínáme s programováním v C#.", 2, "Úvod do C#" }
                });

            migrationBuilder.InsertData(
                table: "User_Course",
                columns: new[] { "Id", "CourseId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 3, 1 },
                    { 3, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Assignment",
                columns: new[] { "Id", "ChapterId", "Description", "DueDate", "TeacherId", "Title" },
                values: new object[] { 1, 1, "Vytvořte jednoduchou SQL tabulku.", new DateTime(2025, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Domácí úkol 1" });

            migrationBuilder.InsertData(
                table: "Chapter_content",
                columns: new[] { "Id", "ChapterId", "TextFile", "Type" },
                values: new object[,]
                {
                    { 1, 1, "uvod_sql.pdf", "Text" },
                    { 2, 2, "select_prikazy.mp4", "Video" },
                    { 3, 3, "uvod_csharp.pdf", "Text" }
                });

            migrationBuilder.InsertData(
                table: "Test",
                columns: new[] { "Id", "ChapterId", "Description", "MaxAttempts", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Ověření znalostí základních SQL příkazů.", 3, "Test základů SQL" },
                    { 2, 2, "Test zaměřený na SELECT příkazy.", 3, "Test SELECT" }
                });

            migrationBuilder.InsertData(
                table: "Assignment_User",
                columns: new[] { "Id", "AssignmentId", "UserId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Assignment_submission",
                columns: new[] { "Id", "AssignmentId", "File", "Text", "UserId" },
                values: new object[] { 1, 1, "users_script.sql", "Vytvořil jsem tabulku users.", 1 });

            migrationBuilder.InsertData(
                table: "Test_Question",
                columns: new[] { "Id", "QuestionId", "TestId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Test_instance",
                columns: new[] { "Id", "Attempt", "StartedAt", "TestId", "UserId" },
                values: new object[] { 1, 1, new DateTime(2025, 3, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_RoleId",
                table: "AppUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ__AppUser__A9D10534C2FFE62C",
                table: "AppUser",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_ChapterId",
                table: "Assignment",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_TeacherId",
                table: "Assignment",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_submission_AssignmentId",
                table: "Assignment_submission",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_submission_UserId",
                table: "Assignment_submission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_User_AssignmentId",
                table: "Assignment_User",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_User_UserId",
                table: "Assignment_User",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_CourseId",
                table: "Chapter",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_content_ChapterId",
                table: "Chapter_content",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_Course_categoryId",
                table: "Course",
                column: "Course_categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_QuestionId",
                table: "Option",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_ChapterId",
                table: "Test",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_instance_TestId",
                table: "Test_instance",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_instance_UserId",
                table: "Test_instance",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Question_QuestionId",
                table: "Test_Question",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Question_TestId",
                table: "Test_Question",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Course_CourseId",
                table: "User_Course",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Course_UserId",
                table: "User_Course",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Assignment_submission");

            migrationBuilder.DropTable(
                name: "Assignment_User");

            migrationBuilder.DropTable(
                name: "Chapter_content");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropTable(
                name: "Test_instance");

            migrationBuilder.DropTable(
                name: "Test_Question");

            migrationBuilder.DropTable(
                name: "User_Course");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Assignment");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.DropTable(
                name: "AccountRole");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Course_category");
        }
    }
}
