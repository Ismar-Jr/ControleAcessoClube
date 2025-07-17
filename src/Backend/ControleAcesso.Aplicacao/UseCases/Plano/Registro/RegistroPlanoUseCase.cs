using AutoMapper;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.Plano;
using ControleAcesso.Excecoes.ExceptionsBase;
using FluentValidation.TestHelper;

namespace ControleAcesso.Aplicacao.UseCases.Plano.Registro;

public class RegistroPlanoUseCase : IRegistroPlanoUseCase
{
    private readonly IPlanoWriteOnlyRepositorio _writeOnlyRepository;
    private readonly IPlanoReadOnlyRepositorio _readOnlyRepository;
    private readonly IUnidadeDeTrabalho _unitOfWork;
    private readonly IMapper _mapper;
    
    public RegistroPlanoUseCase(
        IPlanoWriteOnlyRepositorio writeOnlyRepository,
        IPlanoReadOnlyRepositorio readOnlyRepository,
        IMapper mapper,
        IUnidadeDeTrabalho unitOfWork)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<RespostaRegistroPlanoJson> Execute(RequisicaoRegistroPlanoJson request)
    {
        await Validate(request);
        
        var plano = new Dominio.Entidades.Plano
        {
            Nome = request.Nome!,
            AreasPermitidas = request.IdsAreasPermitidas
                .Select(areaId => new Dominio.Entidades.AreaPermitida { AreaId = areaId })
                .ToList()
        };

        await _writeOnlyRepository.Add(plano);
        await _unitOfWork.Commit();

        return new RespostaRegistroPlanoJson
        {
            Nome = plano.Nome,
        };
    }
    
    private async Task Validate(RequisicaoRegistroPlanoJson request)
    {
        var validator = new ValidacaoRegistroPlano();
        var result = validator.Validate(request);
        
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErroDeValidacao(errorMessages);
        }
    }
}
