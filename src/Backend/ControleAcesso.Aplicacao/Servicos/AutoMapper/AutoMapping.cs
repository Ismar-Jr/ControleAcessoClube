using AutoMapper;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Aplicacao.Servicos.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequisicaoRegistroSocioJson, Dominio.Entidades.Socio>()
            .ForMember(dest => dest.Senha, opt => opt.Ignore()); // Senha tratada à parte

        CreateMap<RequisicaoRegistroPlanoJson, Plano>();

        CreateMap<RequisicaoRegistroAreaJson, Area>();

        CreateMap<RequisicaoTentativaAcessoJson, TentativaAcesso>();
    }

    private void DomainToResponse()
    {
        CreateMap<Dominio.Entidades.Socio, RespostaRegistroSocioJson>();

        CreateMap<Plano, RespostaRegistroPlanoJson>();

        CreateMap<Area, RespostaRegistroAreaJson>();

        CreateMap<TentativaAcesso, RespostaTentativaAcessoJson>();
    }
}