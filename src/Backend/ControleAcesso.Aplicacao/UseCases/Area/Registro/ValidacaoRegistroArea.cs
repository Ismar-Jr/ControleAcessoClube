using ControleAcesso.Comunicacao.Requisicoes;
using FluentValidation;

namespace ControleAcesso.Aplicacao.UseCases.Area.Registro;

public class ValidacaoRegistroArea : AbstractValidator<RequisicaoRegistroAreaJson>

{
    public ValidacaoRegistroArea()
    {
        RuleFor(area => area.Nome).NotEmpty().WithMessage("O nome não pode ser vazio");
    }
}