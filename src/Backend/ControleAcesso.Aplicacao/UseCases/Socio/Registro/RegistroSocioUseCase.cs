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

    /// <summary>
    /// Caso de uso responsável por realizar o registro de um novo usuário.
    /// </summary>
    /// <remarks>
    /// Este caso de uso executa as seguintes etapas:
    /// - Validação dos dados da requisição (incluindo verificação de e-mail duplicado).
    /// - Mapeamento do DTO para entidade de domínio.
    /// - Criptografia da senha do usuário.
    /// - Persistência do usuário no banco de dados.
    /// - Retorno dos dados registrados.
    /// </remarks>
    public class RegistroSocioUseCase : IRegistroSocioUseCase
    {
        private readonly ISocioWriteOnlyRepositorio _writeOnlyRepository;
        private readonly ISocioReadOnlyRepositorio _readOnlyRepository;
        private readonly IUnidadeDeTrabalho _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CriptografiaDeSenha _passwordEncripter;

        /// <summary>
        /// Construtor que recebe as dependências necessárias via injeção.
        /// </summary>
        /// <param name="writeOnlyRepository">Repositório para operações de escrita.</param>
        /// <param name="readOnlyRepository">Repositório para operações de leitura.</param>
        /// <param name="mapper">Componente responsável por transformar DTO em entidade.</param>
        /// <param name="unitOfWork">Gerenciador de transações do banco de dados.</param>
        /// <param name="passwordEncripter">Serviço para criptografar senhas.</param>
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

        /// <summary>
        /// Executa o fluxo principal para registrar um novo usuário.
        /// </summary>
        /// <param name="request">Objeto com os dados necessários para o registro.</param>
        /// <returns>Retorna um objeto contendo os dados do usuário registrado.</returns>
        /// <exception cref="ErrorOnValidationException">Lançada caso a validação dos dados falhe.</exception>
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

        /// <summary>
        /// Realiza a validação dos dados do usuário, incluindo verificação de CPF já cadastrado.
        /// </summary>
        /// <param name="request">Dados do usuário a serem validados.</param>
        /// <exception cref="ErrorOnValidationException">Lançada em caso de erros de validação.</exception>
        private async Task Validate(RequisicaoRegistroSocioJson request)
        {
            var validator = new ValidacaoRegistroSocio();
            var result = validator.Validate(request);
            var cpfExiste = await _readOnlyRepository.ExistActiveUserWithCpf(request.Cpf);
            if (cpfExiste)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, "Cpf já cadastrado"));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErroDeValidacao(errorMessages);
            }
        }
    }
