using AutoMapper;
using ControleAcesso.Aplicacao.Servicos.Criptografia;
using ControleAcesso.Aplicacao.UseCases.Socio.Registro;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.Socio;
using ControleAcesso.Excecoes.ExceptionsBase;
using FluentValidation.Results;

namespace ControleAcesso.Aplicacao.Socio.Registro;

    public class RegistroSocioUseCase : IRegistroSocioUseCase
    {
        private readonly ISocioWriteOnlyRepositorio _writeOnlyRepository;
        private readonly ISocioReadOnlyRepositorio _readOnlyRepository;
        private readonly IUnidadeDeTrabalho _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CriptografiaDeSenha _passwordEncripter;
        
        public RegistroSocioUseCase(
            ISocioWriteOnlyRepositorio writeOnlyRepository,
            ISocioReadOnlyRepositorio readOnlyRepository,
            IMapper mapper,
            IUnidadeDeTrabalho unitOfWork,
            CriptografiaDeSenha passwordEncripter)
        {
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordEncripter = passwordEncripter;
        }
        
        public async Task<RespostaRegistroSocioJson> Execute(RequisicaoRegistroSocioJson request)
        {
            await Validate(request);

            var socio = _mapper.Map<Dominio.Entidades.Socio>(request);
            socio.Senha = _passwordEncripter.Encrypt(request.Senha);

            await _writeOnlyRepository.Add(socio);
            await _unitOfWork.Commit();

            return new RespostaRegistroSocioJson
            {
                Nome = socio.Nome,
                Email = socio.Email,
            };
        }
        
        private async Task Validate(RequisicaoRegistroSocioJson request)
        {
            var validator = new ValidacaoRegistroSocio();
            var result = validator.Validate(request);
            
            var emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (emailExist)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, "Email já cadastrado"));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErroDeValidacao(errorMessages);
            }
            
            var cpfExiste = await _readOnlyRepository.ExistActiveUserWithCpf(request.Cpf);
            if (cpfExiste)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, "CPF já cadastrado"));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErroDeValidacao(errorMessages);
            }
        }
    }
