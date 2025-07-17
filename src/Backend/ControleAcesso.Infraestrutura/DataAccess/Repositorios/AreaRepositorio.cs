using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Repositorios.Area;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleAcesso.Dominio.Repositorios;

namespace ControleAcesso.Infraestrutura.DataAccess.Repositorios
{
    public class AreaRepositorio : IAreaReadOnlyRepositorio, IAreaWriteOnlyRepositorio
    {
        private readonly ControleAcessoDbContext _dbContext;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        public AreaRepositorio(ControleAcessoDbContext dbContext, IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _dbContext = dbContext;
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }

        public async Task Add(Area area)
        {
            await _dbContext.Areas.AddAsync(area);
        }

        public async Task AtualizarAsync(Area area)
        {
            _dbContext.Areas.Update(area);
            await _unidadeDeTrabalho.Commit();
            await Task.CompletedTask; // Método async, mas sem I/O direto
        }

        public async Task<Area?> ObterPorIdAsync(long id)
        {
            return await _dbContext.Areas
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Area>> ListarAtivosAsync()
        {
            return await _dbContext.Areas
                .Where(a => a.Ativo)
                .ToListAsync();
        }
    }
}