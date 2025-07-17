namespace ControleAcesso.Dominio.Repositorios.Socio;

public interface ISocioWriteOnlyRepositorio
{
    /// <summary>
    /// Adiciona um novo sócio.
    /// </summary>
    Task Add(Entidades.Socio socio);

    /// <summary>
    /// Atualiza um sócio existente.
    /// </summary>
    Task AtualizarAsync(Entidades.Socio socio);
}