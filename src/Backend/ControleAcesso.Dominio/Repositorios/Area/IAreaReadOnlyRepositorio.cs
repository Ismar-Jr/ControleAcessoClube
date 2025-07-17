using ControleAcesso.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControleAcesso.Dominio.Repositorios.Area
{
    public interface IAreaReadOnlyRepositorio
    {
        /// <summary>
        /// Obtém uma área pelo seu ID.
        /// </summary>
        /// <param name="id">ID da área.</param>
        /// <returns>A entidade Área ou null se não existir.</returns>
        Task<Entidades.Area?> ObterPorIdAsync(long id);

        /// <summary>
        /// Lista todas as áreas ativas.
        /// </summary>
        /// <returns>Lista de áreas ativas.</returns>
        Task<IEnumerable<Entidades.Area>> ListarAtivosAsync();
    }
}