namespace ControleAcesso.Comunicacao.Respostas;

public class RespostaRegistroSocioJson
{
    public long Id { get; set; }
    public string? Nome { get; set; }
    
    public string? Cpf { get; set; }

    public string? Email { get; set; }
}