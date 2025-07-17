using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;

namespace ControleAcesso.Aplicacao.UseCases.TentativaAcesso.Registro;

public interface IRegistrarTentativaAcessoUseCase
{
    Task<RespostaTentativaAcessoJson> Execute(RequisicaoTentativaAcessoJson request);
}