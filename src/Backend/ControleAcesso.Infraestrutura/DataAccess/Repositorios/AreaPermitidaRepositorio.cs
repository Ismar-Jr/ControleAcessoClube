using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.AreaPermitida.Registro;
using Microsoft.EntityFrameworkCore;

namespace ControleAcesso.Infraestrutura.DataAccess.Repositorios
{
    /// <summary>
    /// Repositório para manipulação de áreas permitidas vinculadas a planos.
    /// </summary>
    public class AreaPermitidaRepositorio : IAreaPermitidaWriteOnlyRepositorio
    {
        private readonly ControleAcessoDbContext _contexto;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        /// <summary>
        /// Inicializa uma nova instância do repositório de áreas permitidas.
        /// </summary>
        /// <param name="contexto">Contexto do banco de dados.</param>
        /// <param name="unidadeDeTrabalho">Unidade de trabalho para commits.</param>
        public AreaPermitidaRepositorio(ControleAcessoDbContext contexto, IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _contexto = contexto;
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }

        /// <summary>
        /// Adiciona uma nova área permitida ao banco.
        /// </summary>
        /// <param name="areaPermitida">Entidade de área permitida a ser adicionada.</param>
        public async Task AdicionarAsync(AreaPermitida areaPermitida)
        {
            await _contexto.AreasPermitidas.AddAsync(areaPermitida);
            await _unidadeDeTrabalho.Commit();
        }

        /// <summary>
        /// Remove todas as áreas permitidas associadas a um plano específico.
        /// </summary>
        /// <param name="planoId">ID do plano cujas áreas permitidas serão removidas.</param>
        public async Task RemoverPorPlanoIdAsync(long planoId)
        {
            var registros = await _contexto.AreasPermitidas
                .Where(ap => ap.PlanoId == planoId)
                .ToListAsync();

            _contexto.AreasPermitidas.RemoveRange(registros);
            await _unidadeDeTrabalho.Commit();
        }
    }
}
