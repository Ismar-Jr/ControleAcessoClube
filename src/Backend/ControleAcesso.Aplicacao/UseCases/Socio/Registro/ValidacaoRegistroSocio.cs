using ControleAcesso.Comunicacao.Requisicoes;
using FluentValidation;

namespace ControleAcesso.Aplicacao.UseCases.Socio.Registro
{
    /// <summary>
    /// Validação das regras para registro de sócio.
    /// </summary>
    public class ValidacaoRegistroSocio : AbstractValidator<RequisicaoRegistroSocioJson>
    {
        public ValidacaoRegistroSocio()
        {
            RuleFor(socio => socio.Nome)
                .NotEmpty()
                .WithMessage("O nome não pode ser vazio");

            RuleFor(socio => socio.Email)
                .EmailAddress()
                .WithMessage("Digite um email válido: exemplo@exemplo");

            RuleFor(socio => socio.Cpf)
                .Must(cpf => cpf != null && cpf.Length == 11 && cpf.All(char.IsDigit))
                .WithMessage("O CPF deve conter exatamente 11 dígitos numéricos");

            RuleFor(socio => socio.Senha)
                .MinimumLength(6)
                .WithMessage("A senha deve ter no mínimo 6 caracteres");
        }
    }
}