using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAgenda.Infraestrutura.Compartilhado.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Update_UsuarioId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TBContatos_Email",
                table: "TBContatos");

            migrationBuilder.DropIndex(
                name: "IX_TBContatos_Telefone",
                table: "TBContatos");

            migrationBuilder.DropIndex(
                name: "IX_TBCategorias_Titulo",
                table: "TBCategorias");

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "TBTarefas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "TBItensTarefa",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "TBDespesas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "TBContatos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "TBCompromissos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "TBCategorias",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            // As tabelas já podem conter dados criados antes da autenticação por usuário.
            // Cria um usuário técnico sem credenciais apenas quando necessário e associa
            // os registros legados a um usuário existente antes de impor as FKs.
            migrationBuilder.Sql("""
                DECLARE @UsuarioLegadoId uniqueidentifier = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa';

                IF NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers)
                   AND (
                       EXISTS (SELECT 1 FROM dbo.TBCategorias)
                       OR EXISTS (SELECT 1 FROM dbo.TBCompromissos)
                       OR EXISTS (SELECT 1 FROM dbo.TBContatos)
                       OR EXISTS (SELECT 1 FROM dbo.TBDespesas)
                       OR EXISTS (SELECT 1 FROM dbo.TBItensTarefa)
                       OR EXISTS (SELECT 1 FROM dbo.TBTarefas)
                   )
                BEGIN
                    INSERT INTO dbo.AspNetUsers
                        (Id, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled,
                         LockoutEnabled, AccessFailedCount)
                    VALUES
                        (@UsuarioLegadoId, 0, 0, 0, 0, 0);
                END;

                IF EXISTS (SELECT 1 FROM dbo.AspNetUsers)
                BEGIN
                    SELECT TOP (1) @UsuarioLegadoId = Id
                    FROM dbo.AspNetUsers
                    ORDER BY CASE WHEN Id = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa' THEN 0 ELSE 1 END, Id;

                    UPDATE dbo.TBCategorias SET UsuarioId = @UsuarioLegadoId WHERE UsuarioId = '00000000-0000-0000-0000-000000000000';
                    UPDATE dbo.TBCompromissos SET UsuarioId = @UsuarioLegadoId WHERE UsuarioId = '00000000-0000-0000-0000-000000000000';
                    UPDATE dbo.TBContatos SET UsuarioId = @UsuarioLegadoId WHERE UsuarioId = '00000000-0000-0000-0000-000000000000';
                    UPDATE dbo.TBDespesas SET UsuarioId = @UsuarioLegadoId WHERE UsuarioId = '00000000-0000-0000-0000-000000000000';
                    UPDATE dbo.TBItensTarefa SET UsuarioId = @UsuarioLegadoId WHERE UsuarioId = '00000000-0000-0000-0000-000000000000';
                    UPDATE dbo.TBTarefas SET UsuarioId = @UsuarioLegadoId WHERE UsuarioId = '00000000-0000-0000-0000-000000000000';
                END;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_TBTarefas_UsuarioId",
                table: "TBTarefas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TBItensTarefa_UsuarioId",
                table: "TBItensTarefa",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TBDespesas_UsuarioId",
                table: "TBDespesas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TBContatos_UsuarioId_Email",
                table: "TBContatos",
                columns: new[] { "UsuarioId", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBContatos_UsuarioId_Telefone",
                table: "TBContatos",
                columns: new[] { "UsuarioId", "Telefone" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBCompromissos_UsuarioId",
                table: "TBCompromissos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TBCategorias_UsuarioId_Titulo",
                table: "TBCategorias",
                columns: new[] { "UsuarioId", "Titulo" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TBCategorias_AspNetUsers_UsuarioId",
                table: "TBCategorias",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TBCompromissos_AspNetUsers_UsuarioId",
                table: "TBCompromissos",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TBContatos_AspNetUsers_UsuarioId",
                table: "TBContatos",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TBDespesas_AspNetUsers_UsuarioId",
                table: "TBDespesas",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TBItensTarefa_AspNetUsers_UsuarioId",
                table: "TBItensTarefa",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TBTarefas_AspNetUsers_UsuarioId",
                table: "TBTarefas",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TBCategorias_AspNetUsers_UsuarioId",
                table: "TBCategorias");

            migrationBuilder.DropForeignKey(
                name: "FK_TBCompromissos_AspNetUsers_UsuarioId",
                table: "TBCompromissos");

            migrationBuilder.DropForeignKey(
                name: "FK_TBContatos_AspNetUsers_UsuarioId",
                table: "TBContatos");

            migrationBuilder.DropForeignKey(
                name: "FK_TBDespesas_AspNetUsers_UsuarioId",
                table: "TBDespesas");

            migrationBuilder.DropForeignKey(
                name: "FK_TBItensTarefa_AspNetUsers_UsuarioId",
                table: "TBItensTarefa");

            migrationBuilder.DropForeignKey(
                name: "FK_TBTarefas_AspNetUsers_UsuarioId",
                table: "TBTarefas");

            migrationBuilder.DropIndex(
                name: "IX_TBTarefas_UsuarioId",
                table: "TBTarefas");

            migrationBuilder.DropIndex(
                name: "IX_TBItensTarefa_UsuarioId",
                table: "TBItensTarefa");

            migrationBuilder.DropIndex(
                name: "IX_TBDespesas_UsuarioId",
                table: "TBDespesas");

            migrationBuilder.DropIndex(
                name: "IX_TBContatos_UsuarioId_Email",
                table: "TBContatos");

            migrationBuilder.DropIndex(
                name: "IX_TBContatos_UsuarioId_Telefone",
                table: "TBContatos");

            migrationBuilder.DropIndex(
                name: "IX_TBCompromissos_UsuarioId",
                table: "TBCompromissos");

            migrationBuilder.DropIndex(
                name: "IX_TBCategorias_UsuarioId_Titulo",
                table: "TBCategorias");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "TBTarefas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "TBItensTarefa");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "TBDespesas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "TBContatos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "TBCompromissos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "TBCategorias");

            migrationBuilder.CreateIndex(
                name: "IX_TBContatos_Email",
                table: "TBContatos",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBContatos_Telefone",
                table: "TBContatos",
                column: "Telefone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBCategorias_Titulo",
                table: "TBCategorias",
                column: "Titulo",
                unique: true);
        }
    }
}
