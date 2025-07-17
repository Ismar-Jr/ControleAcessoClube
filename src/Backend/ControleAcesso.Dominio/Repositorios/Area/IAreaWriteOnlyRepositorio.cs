using System.Threading.Tasks;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Repositorios.Area
{
    public interface IAreaWriteOnlyRepositorio
    {
        /// <summary>
        /// Adiciona uma nova área.
        /// </summary>
        Task Add(Entidades.Area area);

        /// <summary>
        /// Atualiza uma área existente.
        /// </summary>
        Task AtualizarAsync(Entidades.Area area);
    }
}