using System.Collections.Generic;
using System.Threading.Tasks;
using ControleAcesso.Dominio.Entidades; // Ajuste conforme seu projeto

namespace ControleAcesso.Dominio.Repositorios.Plano
{
    public interface IPlanoReadOnlyRepositorio
    {
        /// <summary>
        /// Obtém um plano pelo seu ID.
        /// </summary>
        /// <param name="id">ID do plano.</param>
        /// <returns>O plano encontrado ou null se não existir.</returns>
        Task<Entidades.Plano?> ObterPorIdAsync(long id);

        /// <summary>
        /// Lista todos os planos ativos.
        /// </summary>
        /// <returns>Lista de planos ativos.</returns>
        Task<IEnumerable<Entidades.Plano>> ListarAtivosAsync();
    }
}