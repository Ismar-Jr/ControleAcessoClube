using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.Socio;
using Microsoft.EntityFrameworkCore;

namespace ControleAcesso.Infraestrutura.DataAccess.Repositorios;

/// <summary>
/// Implementação do repositório de sócios, responsável por persistir e consultar dados no banco.
/// Implementa tanto a interface de leitura quanto a de escrita.
/// </summary>
public class SocioRepositorio : ISocioReadOnlyRepositorio, ISocioWriteOnlyRepositorio
{
    private readonly ControleAcessoDbContext _dbContext;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public SocioRepositorio(ControleAcessoDbContext dbContext, IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _dbContext = dbContext;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task Add(Socio socio)
    {
        await _dbContext.Socios.AddAsync(socio);
    }

    public async Task<bool> ExistActiveUserWithCpf(string cpf) =>
        await _dbContext.Socios.AnyAsync(socio => socio.Cpf == cpf && socio.Ativo);
    
    public async Task<bool> ExistActiveUserWithEmail(string email) =>
        await _dbContext.Socios.AnyAsync(socio => socio.Email == email && socio.Ativo);

    /// <summary>
    /// Obtém um sócio pelo ID, incluindo o plano e as áreas permitidas (se necessário).
    /// </summary>
    public async Task<Socio?> ObterPorIdAsync(long id)
    {
        return await _dbContext.Socios
            .Include(s => s.Plano)
            .ThenInclude(p => p.AreasPermitidas)
            .ThenInclude(ap => ap.Area)
            .FirstOrDefaultAsync(s => s.Id == id && s.Ativo);
    }

    /// <summary>
    /// Lista todos os sócios ativos.
    /// </summary>
    public async Task<IEnumerable<Socio>> ListarAtivosAsync()
    {
        return await _dbContext.Socios
            .Where(s => s.Ativo)
            .Include(s => s.Plano)
            .ToListAsync();
    }

    /// <summary>
    /// Atualiza os dados de um sócio.
    /// </summary>
    public async Task AtualizarAsync(Socio socio)
    {
        _dbContext.Socios.Update(socio);
        await _unidadeDeTrabalho.Commit();
        await Task.CompletedTask; // Para manter assinatura async, pois Update é síncrono.
    }
}