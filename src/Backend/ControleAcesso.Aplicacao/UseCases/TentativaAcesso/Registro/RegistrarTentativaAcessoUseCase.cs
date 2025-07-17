using AutoMapper;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Enums;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.Area;
using ControleAcesso.Dominio.Repositorios.Socio;
using ControleAcesso.Dominio.Repositorios.TentativaAcesso;
using ControleAcesso.Excecoes.ExceptionsBase;

namespace ControleAcesso.Aplicacao.UseCases.TentativaAcesso.Registro;

public class RegistrarTentativaAcessoUseCase : IRegistrarTentativaAcessoUseCase
{
    private readonly ISocioReadOnlyRepositorio _socioRepositorio;
    private readonly IAreaReadOnlyRepositorio _areaRepositorio;
    private readonly ITentativaAcessoWriteOnlyRepositorio _tentativaRepositorio;
    private readonly IUnidadeDeTrabalho _unitOfWork;
    private readonly IMapper _mapper;

    public RegistrarTentativaAcessoUseCase(
        ISocioReadOnlyRepositorio socioRepositorio,
        IAreaReadOnlyRepositorio areaRepositorio,
        ITentativaAcessoWriteOnlyRepositorio tentativaRepositorio,
        IMapper mapper,
        IUnidadeDeTrabalho unitOfWork)
    {
        _socioRepositorio = socioRepositorio;
        _areaRepositorio = areaRepositorio;
        _tentativaRepositorio = tentativaRepositorio;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<RespostaTentativaAcessoJson> Execute(RequisicaoTentativaAcessoJson request)
    {
        var socio = await _socioRepositorio.ObterPorIdAsync(request.SocioId)
                    ?? throw new ErroDeValidacao(new List<string> { "Sócio não encontrado." });

        if (!socio.Ativo)
            throw new ErroDeValidacao(new List<string> { "Sócio está inativo." });

        var area = await _areaRepositorio.ObterPorIdAsync(request.AreaId)
                   ?? throw new ErroDeValidacao(new List<string> { "Área não encontrada." });

        if (!area.Ativo)
            throw new ErroDeValidacao(new List<string> { "Área está inativa." });

        var plano = socio.Plano;
        if (plano == null)
            throw new ErroDeValidacao(new List<string> { "Sócio não possui plano associado." });

        var autorizado = plano.AreasPermitidas
            .Any(ap => ap.AreaId == area.Id);

        var tentativa = new Dominio.Entidades.TentativaAcesso
        {
            SocioId = socio.Id,
            AreaId = area.Id,
            DataHora = DateTime.UtcNow,
            Resultado = autorizado ? ResultadoAcesso.Autorizado : ResultadoAcesso.Negado
        };

        await _tentativaRepositorio.Add(tentativa);
        await _unitOfWork.Commit();

        return new RespostaTentativaAcessoJson
        {
            SocioId = socio.Id,
            AreaId = area.Id,
            DataHora = tentativa.DataHora,
            Autorizado = autorizado
        };
    }
}