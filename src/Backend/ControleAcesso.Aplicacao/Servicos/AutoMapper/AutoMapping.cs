using AutoMapper;
using ControleAcesso.Comunicacao.Requisicoes;

namespace ControleAcesso.Aplicacao.Servicos.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
    }

    /// <summary>
    /// Mapeia objetos de requisição para entidades do domínio.
    /// </summary>
    private void RequestToDomain()
    {
        CreateMap<RequisicaoRegistroSocioJson, Dominio.Entidades.Socio>()
            .ForMember(destino => destino.Senha, opt => opt.Ignore()); // Senha será tratada manualmente
    }

    // Espaço reservado para futuros mapeamentos de Response para domínio
    private void ResponseToDomain()
    {
    }
}