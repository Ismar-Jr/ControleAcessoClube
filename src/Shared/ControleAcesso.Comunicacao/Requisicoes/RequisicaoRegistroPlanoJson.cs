namespace ControleAcesso.Comunicacao.Requisicoes;

public class RequisicaoRegistroPlanoJson
{
    public string? Nome { get; set; }

    /// <summary>
    /// Lista de IDs das áreas do clube que este plano permite acesso.
    /// </summary>
    public List<long> IdsAreasPermitidas { get; set; } = new();
}