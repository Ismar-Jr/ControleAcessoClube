namespace ControleAcesso.Comunicacao.Respostas;

public class RespostaTentativaAcessoJson
{
    public long SocioId { get; set; }
    public long AreaId { get; set; }
    public DateTime DataHora { get; set; }
    public bool Autorizado { get; set; }
    
    public string? MensagemResultado { get; set; }
}