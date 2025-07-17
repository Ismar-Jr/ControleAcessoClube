using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Repositorios.TentativaAcesso;

namespace ControleAcesso.Infraestrutura.DataAccess.Repositorios;

public class TentativaAcessoRepositorio : ITentativaAcessoWriteOnlyRepositorio
{
    private readonly ControleAcessoDbContext _dbContext;

    public TentativaAcessoRepositorio(ControleAcessoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(TentativaAcesso tentativa)
    {
        await _dbContext.TentativasAcesso.AddAsync(tentativa);
    }
}