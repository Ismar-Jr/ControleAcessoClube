using AutoMapper;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Repositorios.Area;
using ControleAcesso.Excecoes.ExceptionsBase;
using System.Linq;
using System.Threading.Tasks;
using ControleAcesso.Dominio.Repositorios;

namespace ControleAcesso.Aplicacao.UseCases.Area.Registro
{
    public class RegistroAreaUseCase : IRegistroAreaUseCase
    {
        private readonly IAreaWriteOnlyRepositorio _writeOnlyRepository;
        private readonly IAreaReadOnlyRepositorio _readOnlyRepository;
        private readonly IUnidadeDeTrabalho _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ValidacaoRegistroArea _validator;

        public RegistroAreaUseCase(
            IAreaWriteOnlyRepositorio writeOnlyRepository,
            IAreaReadOnlyRepositorio readOnlyRepository,
            IMapper mapper,
            IUnidadeDeTrabalho unitOfWork)
        {
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator = new ValidacaoRegistroArea();
        }

        public async Task<RespostaRegistroAreaJson> Execute(RequisicaoRegistroAreaJson request)
        {
            await Validate(request);

            var area = _mapper.Map<Dominio.Entidades.Area>(request);

            await _writeOnlyRepository.Add(area);
            await _unitOfWork.Commit();

            return new RespostaRegistroAreaJson
            {
                Id = area.Id, 
                Nome = area.Nome
            };
        }

        private Task Validate(RequisicaoRegistroAreaJson request)
        {
            var result = _validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErroDeValidacao(errorMessages);
            }

            return Task.CompletedTask;
        }
    }
}