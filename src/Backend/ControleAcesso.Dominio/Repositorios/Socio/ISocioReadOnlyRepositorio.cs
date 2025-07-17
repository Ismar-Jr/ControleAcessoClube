using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Repositorios.Socio;

public interface ISocioReadOnlyRepositorio
{
    /// <summary>
    /// Verifica se existe um sócio ativo com o CPF informado.
    /// </summary>
    Task<bool> ExistActiveUserWithCpf(string cpf);
    
    Task<bool> ExistActiveUserWithEmail(string cpf);

    /// <summary>
    /// Retorna um sócio pelo ID (inclusive dados de plano e áreas, se necessário).
    /// </summary>
    Task<Entidades.Socio?> ObterPorIdAsync(long id);

    /// <summary>
    /// Lista todos os sócios ativos.
    /// </summary>
    Task<IEnumerable<Entidades.Socio>> ListarAtivosAsync();
}