namespace ControleAcesso.Comunicacao.Respostas;


/// <summary>
/// Classe usada para representar erros de forma padronizada nas respostas da API.
/// Contém uma lista de mensagens de erro que podem ser retornadas ao cliente.
/// </summary>
public class RespostaErroJson
{
    /// <summary>
    /// Lista de mensagens de erro que descrevem os problemas ocorridos.
    /// </summary>
    public IList<string> Errors { get; set; }

    /// <summary>
    /// Construtor que recebe múltiplas mensagens de erro.
    /// </summary>
    /// <param name="errorMessages">Lista de mensagens de erro.</param>
    public RespostaErroJson(IList<string> errorMessages) => Errors = errorMessages;

    /// <summary>
    /// Construtor que recebe uma única mensagem de erro.
    /// Cria uma lista contendo apenas essa mensagem.
    /// </summary>
    /// <param name="error">Mensagem de erro única.</param>
    public RespostaErroJson(string error)
    {
        Errors = new List<string>
        {
            error
        };
    }
}