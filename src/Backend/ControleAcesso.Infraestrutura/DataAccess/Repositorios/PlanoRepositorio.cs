using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Repositorios.Plano;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleAcesso.Dominio.Repositorios;

namespace ControleAcesso.Infraestrutura.DataAccess.Repositorios
{
    public class PlanoRepositorio : IPlanoReadOnlyRepositorio, IPlanoWriteOnlyRepositorio
    {
        private readonly ControleAcessoDbContext _dbContext;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        public PlanoRepositorio(ControleAcessoDbContext dbContext, IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _dbContext = dbContext;
            _unidadeDeTrabalho = unidadeDeTrabalho;
            
        }

        public async Task Add(Plano plano)
        {
            await _dbContext.Planos.AddAsync(plano);
        }

        public async Task AtualizarAsync(Plano plano)
        {
            _dbContext.Planos.Update(plano);
            await _unidadeDeTrabalho.Commit();
            await Task.CompletedTask; // método async sem I/O, então só pra atender a assinatura
        }

        public async Task<Plano?> ObterPorIdAsync(long id)
        {
           return await _dbContext.Planos
                .Include(p => p.AreasPermitidas)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Plano>> ListarAtivosAsync()
        {
            return await _dbContext.Planos
                .Include(p => p.AreasPermitidas)
                .Where(p => p.Ativo)
                .ToListAsync();
        }
    }
}