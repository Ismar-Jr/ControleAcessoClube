using System.Reflection;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.Socio;
using ControleAcesso.Infraestrutura.DataAccess;
using ControleAcesso.Infraestrutura.DataAccess.Repositorios;
using ControleAcesso.Infraestrutura.Extensoes;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ControleAcesso.Infraestrutura;

/// <summary>
/// Extensão para registrar serviços da camada de Infraestrutura:
/// DbContext, repositórios e FluentMigrator.
/// </summary>
public static class ExtensaoInjecaoDependencia
{
    /// <summary>
    /// Registra todos os serviços relacionados à infraestrutura.
    /// </summary>
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {   
        AddRepositories(services);
        
        if(configuration.IsUnitTestEnviroment())
            return;
        
        AddDbcontext_MySqlServer(services, configuration);
        AddFluentMigrator(services, configuration);
    }

    /// <summary>
    /// Configura o Entity Framework para usar MySQL com versão especificada.
    /// </summary>
    private static void AddDbcontext_MySqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));

        services.AddDbContext<ControleAcessoDbContext>(options =>
            options.UseMySql(connectionString, serverVersion));
    }

    /// <summary>
    /// Registra UnitOfWork e repositórios de usuário com escopo por requisição.
    /// </summary>
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
        services.AddScoped<ISocioWriteOnlyRepositorio, SocioRepositorio>();
        services.AddScoped<ISocioReadOnlyRepositorio, SocioRepositorio>();
    }

    /// <summary>
    /// Configura o FluentMigrator para executar migrações automaticamente.
    /// </summary>
    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");

        services.AddFluentMigratorCore()
            .ConfigureRunner(runner =>
                runner.AddMySql5()
                      .WithGlobalConnectionString(connectionString)
                      .ScanIn(Assembly.Load("ControleAcesso.Infraestrutura"))
                      .For.All());
    }
}