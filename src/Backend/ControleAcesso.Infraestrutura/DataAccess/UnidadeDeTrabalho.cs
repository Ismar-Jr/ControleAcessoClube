using ControleAcesso.Dominio.Repositorios;

namespace ControleAcesso.Infraestrutura.DataAccess;

public class UnidadeDeTrabalho : IUnidadeDeTrabalho
{
    private readonly ControleAcessoDbContext _dbContext;
    
    /// <summary>
    /// Construtor que recebe o contexto do banco via injeção de dependência.
    /// </summary>
    /// <param name="dbContext">Contexto de acesso ao banco MyRecipeBook</param>
    public UnidadeDeTrabalho(ControleAcessoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Persiste todas as alterações feitas no contexto atual no banco de dados.
    /// Método assíncrono que salva as mudanças pendentes.
    /// </summary>
    /// <returns>Task representando a operação assíncrona</returns>
    public async Task Commit() => await _dbContext.SaveChangesAsync();
}