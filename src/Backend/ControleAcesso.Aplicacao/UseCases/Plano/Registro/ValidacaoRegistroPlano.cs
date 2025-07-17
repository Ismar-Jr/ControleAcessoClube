using ControleAcesso.Comunicacao.Requisicoes;
using FluentValidation;

namespace ControleAcesso.Aplicacao.UseCases.Plano.Registro;

public class ValidacaoRegistroPlano : AbstractValidator<RequisicaoRegistroPlanoJson>
{
    public ValidacaoRegistroPlano()
    {
        RuleFor(plano => plano.Nome).NotEmpty().WithMessage("O nome não pode ser vazio");
        RuleFor(plano => plano.IdsAreasPermitidas).NotNull().NotEmpty().WithMessage("Não é possivel criar um plano sem pelo menos uma area vinculada");
    }
}