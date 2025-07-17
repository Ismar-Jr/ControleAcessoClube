using Bogus;
using Bogus.Extensions.Brazil;
using ControleAcesso.Comunicacao.Requisicoes;

namespace CommonUtilities.Requisicoes;

public class RequisicaoRegistroSocioTeste
{
    public static Faker<RequisicaoRegistroSocioJson> Build(int passwordLength = 10)
    {
        return new Faker<RequisicaoRegistroSocioJson>()
            .RuleFor(user => user.Nome, f => f.Person.FirstName)
            .RuleFor(user => user.Email, f => f.Person.Email)
            .RuleFor(user => user.Cpf, f => f.Person.Cpf())
            .RuleFor(user => user.Senha, f => f.Internet.Password(passwordLength))
            .RuleFor(user => user.PlanoId, 1);
    }
}