using AutoMapper;
using ControleAcesso.Aplicacao.Servicos.AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace CommonUtilities.Mapeamento;

public class MapeamentoTeste
{
    /// <summary>
    /// Cria e retorna uma instância de IMapper com o perfil de mapeamento da aplicação.
    /// Ideal para testes que dependem de mapeamento entre DTOs e entidades.
    /// </summary>
    public static IMapper Build()
    {
        var loggerFactory = NullLoggerFactory.Instance;

        return new AutoMapper.MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        }, loggerFactory).CreateMapper();
    }
}