namespace ControleAcesso.Comunicacao.Requisicoes;

public class RequisicaoRegistroSocioJson
{
    public long Id { get; set; }
    public string? Nome { get; set; }
    public string? Cpf { get; set; }
    public string? Email { get; set; }
    public string? Senha { get; set; }

    public long PlanoId { get; set; }

}