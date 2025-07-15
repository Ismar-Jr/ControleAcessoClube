using ControleAcesso.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ControleAcesso.Infraestrutura.DataAccess;

public class ControleAcessoDbContext : DbContext
{
    /// <summary>
    /// Construtor do DbContext, recebe as opções de configuração (como connection string, provedor, etc).
    /// </summary>
    /// <param name="options">Opções de configuração do contexto.</param>
    public ControleAcessoDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Socio> Socios { get; set; }
    public DbSet<Plano> Planos { get; set; }
    

    /// <summary>
    /// Método chamado automaticamente pelo Entity Framework na criação do modelo.
    /// Aqui aplicamos todas as configurações de mapeamento definidas no assembly atual,
    /// como configurações personalizadas via Fluent API.
    /// </summary>
    /// <param name="modelBuilder">Construtor de modelos utilizado para configurar as entidades.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ControleAcessoDbContext).Assembly);
    }
}