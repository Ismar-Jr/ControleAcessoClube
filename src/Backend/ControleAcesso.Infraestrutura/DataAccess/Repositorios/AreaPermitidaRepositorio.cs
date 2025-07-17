using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.AreaPermitida.Registro;
using Microsoft.EntityFrameworkCore;

namespace ControleAcesso.Infraestrutura.DataAccess.Repositorios;

public class AreaPermitidaRepositorio : IAreaPermitidaWriteOnlyRepositorio
{
    private readonly ControleAcessoDbContext _dbContext;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public AreaPermitidaRepositorio(ControleAcessoDbContext dbContext, IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _dbContext = dbContext;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task AdicionarAsync(AreaPermitida areaPermitida)
    {
        await _dbContext.AreasPermitidas.AddAsync(areaPermitida);
        await _unidadeDeTrabalho.Commit();
    }

    public async Task RemoverPorPlanoIdAsync(long planoId)
    {
        var registros = await _dbContext.AreasPermitidas
            .Where(ap => ap.PlanoId == planoId)
            .ToListAsync();

        _dbContext.AreasPermitidas.RemoveRange(registros);
        await _unidadeDeTrabalho.Commit();
    }
}