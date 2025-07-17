using ControleAcesso.Comunicacao.Requisicoes;

public interface IAtualizarSocioUseCase
{
    Task Execute(long id, RequisicaoRegistroSocioJson request);
}