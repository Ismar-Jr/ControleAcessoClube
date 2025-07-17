using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;

namespace ControleAcesso.Aplicacao.UseCases.Socio.Registro;

public interface IRegistroSocioUseCase
{
    Task<RespostaRegistroSocioJson> Execute(RequisicaoRegistroSocioJson request);
}