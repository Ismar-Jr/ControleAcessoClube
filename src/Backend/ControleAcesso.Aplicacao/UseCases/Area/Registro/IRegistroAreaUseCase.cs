using ControleAcesso.Comunicacao.Requisicoes;

namespace ControleAcesso.Aplicacao.UseCases.Area.Registro;

public interface IRegistroAreaUseCase
{
    public Task<object?> Execute(RequisicaoRegistroAreaJson request);
}