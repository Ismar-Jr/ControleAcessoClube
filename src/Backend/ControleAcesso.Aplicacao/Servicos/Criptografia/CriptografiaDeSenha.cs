namespace ControleAcesso.Aplicacao.Servicos.Criptografia;

public class CriptografiaDeSenha
{
    /// <summary>
    /// Gera um hash seguro da senha usando bcrypt.
    /// O salt é gerado automaticamente e embutido no resultado.
    /// </summary>
    /// <param name="password">Senha em texto puro informada pelo usuário.</param>
    /// <returns>Hash seguro contendo salt embutido.</returns>
    public string Encrypt(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verifica se a senha informada confere com o hash armazenado.
    /// Internamente, o bcrypt extrai o salt do hash e aplica novamente.
    /// </summary>
    /// <param name="password">Senha em texto puro fornecida no login.</param>
    /// <param name="hashedPassword">Hash previamente gerado e armazenado no banco.</param>
    /// <returns>True se a senha estiver correta, false caso contrário.</returns>
    public bool Verify(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}