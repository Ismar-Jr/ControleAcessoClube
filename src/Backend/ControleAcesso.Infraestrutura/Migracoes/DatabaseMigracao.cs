using Microsoft.Extensions.DependencyInjection;
using Dapper;
using MySqlConnector;
using FluentMigrator.Runner;


namespace ControleAcesso.Infraestrutura.Migracoes;

public static class DatabaseMigracao
{
    /// <summary>
    /// Executa o processo completo de migração do banco.
    /// </summary>
    /// <param name="connectionString">String de conexão com o banco, incluindo o schema.</param>
    /// <param name="serviceProvider">ServiceProvider com FluentMigrator configurado.</param>
    public static void Migrate(string? connectionString, IServiceProvider serviceProvider)
    {
        EnsureDatabaseCreated(connectionString);
        MigrationDatabase(serviceProvider);
    }

    /// <summary>
    /// Garante que o schema exista, criando-o se necessário.
    /// </summary>
    private static void EnsureDatabaseCreated(string? connectionString)
    {
        var builder = new MySqlConnectionStringBuilder(connectionString);
        var databaseName = builder.Database;

        builder.Remove("Database");

        using var connection = new MySqlConnection(builder.ConnectionString);

        var parameters = new DynamicParameters();
        parameters.Add("@Name", databaseName);

        var exists = connection.Query(
            "SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @Name",
            parameters
        ).Any();

        if (!exists)
        {
            connection.Execute($"CREATE DATABASE `{databaseName}`");
        }
    }

    /// <summary>
    /// Aplica todas as migrações pendentes no banco.
    /// </summary>
    private static void MigrationDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();
    }
}