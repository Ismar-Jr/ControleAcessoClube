using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Repositorios.Plano;

namespace ControleAcesso.Infraestrutura.DataAccess.Repositorios;

public class PlanoRepositorio : IPlanoReadOnlyRepositorio, IPlanoWriteOnlyRepositorio
{
    // Referência para o contexto do banco de dados (EF Core), injetado via construtor.
    private readonly ControleAcessoDbContext _dbContext;
    private IPlanoWriteOnlyRepositorio _planoWriteOnlyRepositorioImplementation;

    /// <summary>
    /// Construtor que injeta o DbContext da aplicação.
    /// </summary>
    /// <param name="dbContext">Instância do contexto do banco de dados.</param>
    public PlanoRepositorio(ControleAcessoDbContext dbContext) => _dbContext = dbContext;

    /// <summary>
    /// Adiciona um novo usuário ao banco de dados.
    /// A operação ainda não é persistida no banco até que SaveChangesAsync seja chamado (normalmente pela Unit of Work).
    /// </summary>
    /// <param name="plano">Entidade de usuário a ser adicionada.</param>
    public async Task Add(Plano plano)
    {
        await _dbContext.Planos.AddAsync(plano);
    }

}