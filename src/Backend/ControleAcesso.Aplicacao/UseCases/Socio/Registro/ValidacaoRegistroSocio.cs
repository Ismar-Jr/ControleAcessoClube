using ControleAcesso.Comunicacao.Requisicoes;

namespace ControleAcesso.Aplicacao.Socio.Registro;
using FluentValidation;

public class ValidacaoRegistroSocio : AbstractValidator<RequisicaoRegistroSocioJson>
{
    public ValidacaoRegistroSocio()
    {
        RuleFor(socio => socio.Nome).NotEmpty().WithMessage("O nome não pode ser vazio");

        RuleFor(socio => socio.Email).EmailAddress().WithMessage("Digite um email válido: exemplo@exemplo");
        
       RuleFor(socio => socio.Cpf)
            .Must(cpf => cpf != null && cpf.Length == 11 && cpf.All(char.IsDigit))
            .WithMessage("O CPF deve conter exatamente 11 dígitos numéricos");


        RuleFor(socio => socio.Senha.Length).GreaterThanOrEqualTo(6).WithMessage("A senha deve ter no mínimo 6 caracteres");
    }
}