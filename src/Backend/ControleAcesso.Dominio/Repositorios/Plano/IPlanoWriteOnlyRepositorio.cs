using System.Threading.Tasks;
using ControleAcesso.Dominio.Entidades; // ajuste conforme seu projeto

namespace ControleAcesso.Dominio.Repositorios.Plano
{
    public interface IPlanoWriteOnlyRepositorio
    {
        /// <summary>
        /// Adiciona um novo plano.
        /// </summary>
        Task Add(Entidades.Plano plano);

        /// <summary>
        /// Atualiza um plano existente.
        /// </summary>
        Task AtualizarAsync(Entidades.Plano plano);
    }
}