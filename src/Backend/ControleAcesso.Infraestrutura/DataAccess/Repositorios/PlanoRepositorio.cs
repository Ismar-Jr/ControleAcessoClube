using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Repositorios.Plano;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleAcesso.Dominio.Repositorios;

namespace ControleAcesso.Infraestrutura.DataAccess.Repositorios
{
    /// <summary>
    /// Repositório para manipulação da entidade Plano.
    /// Implementa operações de leitura e escrita.
    /// </summary>
    public class PlanoRepositorio : IPlanoReadOnlyRepositorio, IPlanoWriteOnlyRepositorio
    {
        private readonly ControleAcessoDbContext _contexto;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        /// <summary>
        /// Inicializa uma nova instância do repositório de Plano.
        /// </summary>
        /// <param name="contexto">Contexto do banco de dados.</param>
        /// <param name="unidadeDeTrabalho">Unidade de trabalho para commits.</param>
        public PlanoRepositorio(ControleAcessoDbContext contexto, IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _contexto = contexto;
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }
        
        public async Task Add(Plano plano)
        {
            await _contexto.Planos.AddAsync(plano);
        }
        
        public async Task AtualizarAsync(Plano plano)
        {
            _contexto.Planos.Update(plano);
            await _unidadeDeTrabalho.Commit();
            await Task.CompletedTask; // Método async sem I/O direto, para atender à assinatura
        }
        
        public async Task<Plano?> ObterPorIdAsync(long id)
        {
           return await _contexto.Planos
                .Include(p => p.AreasPermitidas)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<IEnumerable<Plano>> ListarAtivosAsync()
        {
            return await _contexto.Planos
                .Include(p => p.AreasPermitidas)
                .Where(p => p.Ativo)
                .ToListAsync();
        }
    }
}
