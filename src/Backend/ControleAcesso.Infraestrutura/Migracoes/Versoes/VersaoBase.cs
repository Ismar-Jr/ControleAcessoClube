using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace ControleAcesso.Infraestrutura.Migracoes.Versoes;

public abstract class VersaoBase : ForwardOnlyMigration
{
    /// <summary>
    /// Cria uma nova tabela com colunas padrão (Id, CreatedOn, Active),
    /// e retorna o builder para que outras colunas sejam adicionadas pela migração específica.
    /// </summary>
    /// <param name="tableName">Nome da tabela a ser criada.</param>
    /// <returns>Builder para continuação da definição de colunas.</returns>
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string tableName)
    {
        return Create.Table(tableName)
            // Coluna "Id": chave primária do tipo inteiro longo, auto-incrementável.
            .WithColumn("Id").AsInt64().Identity().PrimaryKey()

            // Coluna "CreatedOn": armazena a data de criação do registro.
            .WithColumn("CriadoEm").AsDateTime().NotNullable()

            // Coluna "Active": define se o registro está ativo ou não (soft delete).
            .WithColumn("Ativo").AsBoolean().NotNullable();
    }
}