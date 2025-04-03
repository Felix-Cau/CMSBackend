using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAlphaDbContextDbSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAddressEntity_Clients_ClientId",
                table: "ClientAddressEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientContactInformationEntity_Clients_ClientId",
                table: "ClientContactInformationEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientContactInformationEntity",
                table: "ClientContactInformationEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientAddressEntity",
                table: "ClientAddressEntity");

            migrationBuilder.RenameTable(
                name: "ClientContactInformationEntity",
                newName: "ClientContacts");

            migrationBuilder.RenameTable(
                name: "ClientAddressEntity",
                newName: "ClientAddresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientContacts",
                table: "ClientContacts",
                column: "ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientAddresses",
                table: "ClientAddresses",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAddresses_Clients_ClientId",
                table: "ClientAddresses",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientContacts_Clients_ClientId",
                table: "ClientContacts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAddresses_Clients_ClientId",
                table: "ClientAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientContacts_Clients_ClientId",
                table: "ClientContacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientContacts",
                table: "ClientContacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientAddresses",
                table: "ClientAddresses");

            migrationBuilder.RenameTable(
                name: "ClientContacts",
                newName: "ClientContactInformationEntity");

            migrationBuilder.RenameTable(
                name: "ClientAddresses",
                newName: "ClientAddressEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientContactInformationEntity",
                table: "ClientContactInformationEntity",
                column: "ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientAddressEntity",
                table: "ClientAddressEntity",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAddressEntity_Clients_ClientId",
                table: "ClientAddressEntity",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientContactInformationEntity_Clients_ClientId",
                table: "ClientContactInformationEntity",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
