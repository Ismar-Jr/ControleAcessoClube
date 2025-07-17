using FluentValidation;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Dominio.Repositorios.Plano;
using ControleAcesso.Dominio.Repositorios.Socio;

public class ValidacaoAtualizacaoSocio : AbstractValidator<RequisicaoAtualizacaoSocioJson>
{
    private readonly ISocioReadOnlyRepositorio _repositorioLeitura;
    private readonly IPlanoReadOnlyRepositorio _planoReadOnlyRepositorio;

    public ValidacaoAtualizacaoSocio(
        ISocioReadOnlyRepositorio repositorioLeitura,
        IPlanoReadOnlyRepositorio planoReadOnlyRepositorio)
    {
        _repositorioLeitura = repositorioLeitura;
        _planoReadOnlyRepositorio = planoReadOnlyRepositorio;

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Digite um email válido: exemplo@exemplo")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Cpf)
            .Matches(@"^\d{11}$").WithMessage("CPF deve ter 11 dígitos.")
            .When(x => !string.IsNullOrEmpty(x.Cpf));

        RuleFor(x => x.Email)
            .MustAsync(async (request, email, cancellation) =>
                !await _repositorioLeitura.ExistActiveUserWithEmail(email, request.Id))
            .WithMessage("Email já cadastrado.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Cpf)
            .MustAsync(async (request, cpf, cancellation) =>
                !await _repositorioLeitura.ExistActiveUserWithCpf(cpf, request.Id))
            .WithMessage("CPF já cadastrado.")
            .When(x => !string.IsNullOrEmpty(x.Cpf));

        RuleFor(x => x.Senha)
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Senha));

        RuleFor(x => x.PlanoId)
            .MustAsync(async (planoId, cancellation) =>
            {
                var plano = await _planoReadOnlyRepositorio.ObterPorIdAsync(planoId);
                return plano != null && plano.Ativo;
            })
            .WithMessage("Plano informado não existe ou está inativo.")
            .When(x => x.PlanoId > 0);
    }
}
