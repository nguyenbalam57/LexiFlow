using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LexiFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeletedItems",
                columns: table => new
                {
                    DeletedItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BackupData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedItems", x => x.DeletedItemID);
                    table.ForeignKey(
                        name: "FK_DeletedItems_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "SyncConflicts",
                columns: table => new
                {
                    ConflictID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    ClientData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServerData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServerUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConflictType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolutionMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DetectedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncConflicts", x => x.ConflictID);
                    table.ForeignKey(
                        name: "FK_SyncConflicts_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "SyncMetadata",
                columns: table => new
                {
                    SyncMetadataID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    LastSyncTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalItemsSynced = table.Column<int>(type: "int", nullable: false),
                    NeedsFullSync = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncMetadata", x => x.SyncMetadataID);
                    table.ForeignKey(
                        name: "FK_SyncMetadata_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeletedItems_UserID",
                table: "DeletedItems",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SyncConflicts_UserID",
                table: "SyncConflicts",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SyncMetadata_UserID",
                table: "SyncMetadata",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedItems");

            migrationBuilder.DropTable(
                name: "SyncConflicts");

            migrationBuilder.DropTable(
                name: "SyncMetadata");
        }
    }
}
