using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.Socio;
using Microsoft.EntityFrameworkCore;

namespace ControleAcesso.Infraestrutura.DataAccess.Repositorios
{
    /// <summary>
    /// Repositório para manipulação da entidade Sócio.
    /// Implementa operações de leitura e escrita no banco de dados.
    /// </summary>
    public class SocioRepositorio : ISocioReadOnlyRepositorio, ISocioWriteOnlyRepositorio
    {
        private readonly ControleAcessoDbContext _dbContext;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        /// <summary>
        /// Inicializa uma nova instância do repositório de Sócio.
        /// </summary>
        /// <param name="dbContext">Contexto do banco de dados.</param>
        /// <param name="unidadeDeTrabalho">Unidade de trabalho para commits.</param>
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
        
        public async Task<Socio?> ObterPorIdAsync(long id)
        {
            return await _dbContext.Socios
                .Include(s => s.Plano)
                .ThenInclude(p => p.AreasPermitidas)
                .ThenInclude(ap => ap.Area)
                .FirstOrDefaultAsync(s => s.Id == id && s.Ativo);
        }
        
        public async Task<IEnumerable<Socio>> ListarAtivosAsync()
        {
            return await _dbContext.Socios
                .Where(s => s.Ativo)
                .Include(s => s.Plano)
                .ToListAsync();
        }
        
        public async Task AtualizarAsync(Socio socio)
        {
            _dbContext.Socios.Update(socio);
            await _unidadeDeTrabalho.Commit();
            await Task.CompletedTask; // Mantém assinatura async por padrão.
        }
    }
}
