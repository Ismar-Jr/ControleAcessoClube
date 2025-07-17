using ControleAcesso.Aplicacao.Servicos.AutoMapper;
using ControleAcesso.Aplicacao.Servicos.Criptografia;
using ControleAcesso.Aplicacao.UseCases.Socio.Registro;
using ControleAcesso.Aplicacao.UseCases.Area.Registro;
using ControleAcesso.Aplicacao.UseCases.Plano.Registro;
using ControleAcesso.Aplicacao.UseCases.Socio.Atualizacao;
using ControleAcesso.Aplicacao.UseCases.TentativaAcesso.Registro;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace ControleAcesso.Aplicacao;

public static class ExtensaoInjecaoDependencia
{
    /// <summary>
    /// Registra os serviços da aplicação, como criptografia, AutoMapper e casos de uso.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    public static void AddApplication(this IServiceCollection services)
    {
        AddPasswordEncripter(services);
        AddAutoMapper(services);
        AddUseCases(services);
    }

    /// <summary>
    /// Registra o AutoMapper com os perfis de mapeamento definidos.
    /// </summary>
    private static void AddAutoMapper(IServiceCollection services)
    {
        var loggerFactory = NullLoggerFactory.Instance;

        services.AddScoped(option => new AutoMapper.MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        }, loggerFactory).CreateMapper());
    }

    /// <summary>
    /// Registra os casos de uso da camada de aplicação.
    /// </summary>
    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegistroSocioUseCase, RegistroSocioUseCase>();
        services.AddScoped<IRegistroPlanoUseCase, RegistroPlanoUseCase>();
        services.AddScoped<IRegistroAreaUseCase, RegistroAreaUseCase>();
        services.AddScoped<IRegistrarTentativaAcessoUseCase, RegistrarTentativaAcessoUseCase>();
        services.AddScoped<IAtualizarSocioUseCase, AtualizarSocioUseCase>();

    }

    /// <summary>
    /// Registra o serviço de criptografia de senhas.
    /// </summary>
    private static void AddPasswordEncripter(IServiceCollection services)
    {
        services.AddScoped<CriptografiaDeSenha>();
    }
}
