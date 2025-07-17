using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Repositorios.Area;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleAcesso.Dominio.Repositorios;

namespace ControleAcesso.Infraestrutura.DataAccess.Repositorios
{
    /// <summary>
    /// Repositório para manipulação da entidade Área.
    /// Implementa operações de leitura e escrita.
    /// </summary>
    public class AreaRepositorio : IAreaReadOnlyRepositorio, IAreaWriteOnlyRepositorio
    {
        private readonly ControleAcessoDbContext _contexto;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        /// <summary>
        /// Inicializa uma nova instância do repositório de Área.
        /// </summary>
        /// <param name="contexto">Contexto do banco de dados.</param>
        /// <param name="unidadeDeTrabalho">Unidade de trabalho para commits.</param>
        public AreaRepositorio(ControleAcessoDbContext contexto, IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _contexto = contexto;
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }

        /// <summary>
        /// Adiciona uma nova área no banco de dados.
        /// </summary>
        /// <param name="area">Entidade Área a ser adicionada.</param>
        public async Task Add(Area area)
        {
            await _contexto.Areas.AddAsync(area);
        }

        /// <summary>
        /// Atualiza uma área existente no banco.
        /// </summary>
        /// <param name="area">Entidade Área a ser atualizada.</param>
        public async Task AtualizarAsync(Area area)
        {
            _contexto.Areas.Update(area);
            await _unidadeDeTrabalho.Commit();
            await Task.CompletedTask; // Método assíncrono, mas sem operação de I/O direta aqui
        }

        /// <summary>
        /// Obtém uma área pelo seu ID.
        /// </summary>
        /// <param name="id">ID da área.</param>
        /// <returns>Área encontrada ou null se não existir.</returns>
        public async Task<Area?> ObterPorIdAsync(long id)
        {
            return await _contexto.Areas
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        /// <summary>
        /// Lista todas as áreas que estão ativas.
        /// </summary>
        /// <returns>Lista de áreas ativas.</returns>
        public async Task<IEnumerable<Area>> ListarAtivosAsync()
        {
            return await _contexto.Areas
                .Where(a => a.Ativo)
                .ToListAsync();
        }
    }
}
