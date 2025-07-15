using AutoMapper;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.Plano;

namespace ControleAcesso.Aplicacao.UseCases.Plano.Registro;

public class RegistroPlanoUseCase : IRegistroPlanoUseCase
{
        private readonly IPlanoWriteOnlyRepositorio _writeOnlyRepository;
        private readonly IPlanoReadOnlyRepositorio _readOnlyRepository;
        private readonly IUnidadeDeTrabalho _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor que recebe as dependências necessárias via injeção.
        /// </summary>
        /// <param name="writeOnlyRepository">Repositório para operações de escrita.</param>
        /// <param name="readOnlyRepository">Repositório para operações de leitura.</param>
        /// <param name="mapper">Componente responsável por transformar DTO em entidade.</param>
        /// <param name="unitOfWork">Gerenciador de transações do banco de dados.</param>
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

        /// <summary>
        /// Executa o fluxo principal para registrar um novo usuário.
        /// </summary>
        /// <param name="request">Objeto com os dados necessários para o registro.</param>
        /// <returns>Retorna um objeto contendo os dados do usuário registrado.</returns>
        /// <exception cref="ErrorOnValidationException">Lançada caso a validação dos dados falhe.</exception>
        public async Task<RespostaRegistroPlanoJson> Execute(RequisicaoRegistroPlanoJson request)
        {

            var plano = _mapper.Map<Dominio.Entidades.Plano>(request);

            await _writeOnlyRepository.Add(plano);
            await _unitOfWork.Commit();

            return new RespostaRegistroPlanoJson
            {
                Nome = plano.Nome,
            };
        }
        
}