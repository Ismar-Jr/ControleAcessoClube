using FluentMigrator;

namespace ControleAcesso.Infraestrutura.Migracoes.Versoes;

/// <summary>
/// Versão de migração responsável por criar a tabela "Users" no banco de dados.
/// Esta versão inicial inclui as colunas:
/// - Name: nome do usuário
/// - Email: endereço de e-mail
/// - Password: senha criptografada
/// </summary>
[Migration(DatabaseVersions.TABLE_USER, "Criar tabela para salvar as informações dos Socios")]
public class Versao001 : VersaoBase
{
    /// <summary>
    /// Método chamado ao aplicar a migração (método "Up" do FluentMigrator).
    /// Responsável por criar a estrutura da tabela "Users".
    /// </summary>
    public override void Up()
    {
        CreateTable("Socios")
            .WithColumn("Nome").AsString(255).NotNullable()
            
            .WithColumn("Email").AsString(255).NotNullable()
            
            .WithColumn("CPF").AsString(11).NotNullable()
            
            .WithColumn("Senha").AsString(2000).NotNullable();
    }
}