using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;

namespace ControleAcesso.Aplicacao.UseCases.Area.Registro;

public interface IRegistroAreaUseCase
{
    public Task<RespostaRegistroAreaJson> Execute(RequisicaoRegistroAreaJson request);
}