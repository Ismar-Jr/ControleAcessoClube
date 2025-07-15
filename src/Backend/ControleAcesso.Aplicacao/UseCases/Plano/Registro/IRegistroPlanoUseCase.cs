using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;

namespace ControleAcesso.Aplicacao.UseCases.Plano.Registro;

public interface IRegistroPlanoUseCase
{
    Task<RespostaRegistroPlanoJson> Execute(RequisicaoRegistroPlanoJson request);
}