using AutoMapper;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Aplicacao.Servicos.AutoMapper;

/// <summary>
/// Perfil AutoMapper para mapeamentos entre requisições, entidades de domínio e respostas.
/// </summary>
public class AutoMapping : Profile
{
    /// <summary>
    /// Configura os mapeamentos no construtor.
    /// </summary>
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    /// <summary>
    /// Configura os mapeamentos de requisições para entidades de domínio.
    /// </summary>
    private void RequestToDomain()
    {
        CreateMap<RequisicaoRegistroSocioJson, Dominio.Entidades.Socio>()
            .ForMember(dest => dest.Senha, opt => opt.Ignore()); // Senha tratada à parte

        CreateMap<RequisicaoRegistroPlanoJson, Plano>();

        CreateMap<RequisicaoRegistroAreaJson, Area>();

        CreateMap<RequisicaoTentativaAcessoJson, TentativaAcesso>();
    }

    /// <summary>
    /// Configura os mapeamentos de entidades de domínio para respostas.
    /// </summary>
    private void DomainToResponse()
    {
        CreateMap<Dominio.Entidades.Socio, RespostaRegistroSocioJson>();

        CreateMap<Plano, RespostaRegistroPlanoJson>();

        CreateMap<Area, RespostaRegistroAreaJson>();

        CreateMap<TentativaAcesso, RespostaTentativaAcessoJson>();
    }
}