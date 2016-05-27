using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace HSDraft_v2.Migrations
{
    public partial class initial : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    AccessFailedCount = table.Column(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column(type: "nvarchar(max)", nullable: true),
                    Email = table.Column(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column(type: "bit", nullable: false),
                    Id = table.Column(type: "nvarchar(450)", nullable: true),
                    LockoutEnabled = table.Column(type: "bit", nullable: false),
                    LockoutEnd = table.Column(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column(type: "bit", nullable: false),
                    SecurityStamp = table.Column(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column(type: "bit", nullable: false),
                    UserName = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });
            migration.CreateTable(
                name: "Card",
                columns: table => new
                {
                    AddedToDraft = table.Column(type: "int", nullable: false),
                    Attack = table.Column(type: "int", nullable: false),
                    Cost = table.Column(type: "int", nullable: false),
                    Durability = table.Column(type: "int", nullable: false),
                    Health = table.Column(type: "int", nullable: false),
                    Id = table.Column(type: "nvarchar(450)", nullable: true),
                    Name = table.Column(type: "nvarchar(max)", nullable: true),
                    PlayerClass = table.Column(type: "int", nullable: false),
                    Race = table.Column(type: "nvarchar(max)", nullable: true),
                    Rarity = table.Column(type: "int", nullable: false),
                    Text = table.Column(type: "nvarchar(max)", nullable: true),
                    Type = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.Id);
                });
            migration.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    ConcurrencyStamp = table.Column(type: "nvarchar(max)", nullable: true),
                    Id = table.Column(type: "nvarchar(450)", nullable: true),
                    Name = table.Column(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });
            migration.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    ClaimType = table.Column(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column(type: "nvarchar(max)", nullable: true),
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    UserId = table.Column(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        columns: x => x.UserId,
                        referencedTable: "AspNetUsers",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column(type: "nvarchar(450)", nullable: true),
                    ProviderDisplayName = table.Column(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        columns: x => x.UserId,
                        referencedTable: "AspNetUsers",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    ClaimType = table.Column(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column(type: "nvarchar(max)", nullable: true),
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    RoleId = table.Column(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        columns: x => x.RoleId,
                        referencedTable: "AspNetRoles",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    RoleId = table.Column(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        columns: x => x.RoleId,
                        referencedTable: "AspNetRoles",
                        referencedColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        columns: x => x.UserId,
                        referencedTable: "AspNetUsers",
                        referencedColumn: "Id");
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropTable("AspNetUsers");
            migration.DropTable("Card");
            migration.DropTable("AspNetRoles");
            migration.DropTable("AspNetRoleClaims");
            migration.DropTable("AspNetUserClaims");
            migration.DropTable("AspNetUserLogins");
            migration.DropTable("AspNetUserRoles");
        }
    }
}
