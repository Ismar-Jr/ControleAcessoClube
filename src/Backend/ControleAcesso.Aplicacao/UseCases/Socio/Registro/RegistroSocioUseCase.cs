using AutoMapper;
using ControleAcesso.Aplicacao.Servicos.Criptografia;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.Plano;
using ControleAcesso.Dominio.Repositorios.Socio;
using ControleAcesso.Excecoes.ExceptionsBase;
using FluentValidation.Results;
using System.Linq;
using System.Threading.Tasks;

namespace ControleAcesso.Aplicacao.UseCases.Socio.Registro
{
    /// <summary>
    /// Caso de uso para registro de um novo sócio.
    /// </summary>
    public class RegistroSocioUseCase : IRegistroSocioUseCase
    {
        private readonly ISocioWriteOnlyRepositorio _repositorioEscrita;
        private readonly ISocioReadOnlyRepositorio _repositorioLeitura;
        private readonly IPlanoReadOnlyRepositorio _planoReadOnlyRepositorio;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
        private readonly IMapper _mapper;
        private readonly CriptografiaDeSenha _criptografiaSenha;
        
        /// <summary>
        /// Construtor com injeção de dependências.
        /// </summary>
        public RegistroSocioUseCase(
            ISocioWriteOnlyRepositorio repositorioEscrita,
            ISocioReadOnlyRepositorio repositorioLeitura,
            IPlanoReadOnlyRepositorio planoReadOnlyRepositorio,
            IMapper mapper,
            IUnidadeDeTrabalho unidadeDeTrabalho,
            CriptografiaDeSenha criptografiaSenha)
        {
            _repositorioEscrita = repositorioEscrita;
            _repositorioLeitura = repositorioLeitura;
            _planoReadOnlyRepositorio = planoReadOnlyRepositorio;
            _mapper = mapper;
            _unidadeDeTrabalho = unidadeDeTrabalho;
            _criptografiaSenha = criptografiaSenha;
        }
        
        /// <summary>
        /// Executa o registro do sócio a partir da requisição.
        /// </summary>
        /// <param name="request">Dados do sócio a serem cadastrados.</param>
        /// <returns>Resposta com dados do sócio cadastrado.</returns>
        public async Task<RespostaRegistroSocioJson> Execute(RequisicaoRegistroSocioJson request)
        {
            await Validar(request);

            var socio = _mapper.Map<Dominio.Entidades.Socio>(request);
            socio.Senha = _criptografiaSenha.Encrypt(request.Senha);

            await _repositorioEscrita.Add(socio);
            await _unidadeDeTrabalho.Commit();

            return new RespostaRegistroSocioJson
            {
                Nome = socio.Nome,
                Email = socio.Email,
            };
        }
        
        /// <summary>
        /// Valida os dados da requisição para registro do sócio,
        /// verificando duplicidade de email, CPF e existência/ativação do plano.
        /// </summary>
        /// <param name="request">Dados da requisição.</param>
        /// <exception cref="ErroDeValidacao">Disparada quando dados inválidos são encontrados.</exception>
        private async Task Validar(RequisicaoRegistroSocioJson request)
        {
            var validador = new ValidacaoRegistroSocio();
            var resultado = validador.Validate(request);

            // Valida email duplicado
            var emailExistente = await _repositorioLeitura.ExistActiveUserWithEmail(request.Email);
            if (emailExistente)
            {
                resultado.Errors.Add(new ValidationFailure(string.Empty, "Email já cadastrado"));
            }

            // Valida CPF duplicado
            var cpfExistente = await _repositorioLeitura.ExistActiveUserWithCpf(request.Cpf);
            if (cpfExistente)
            {
                resultado.Errors.Add(new ValidationFailure(string.Empty, "CPF já cadastrado"));
            }

            // Valida existência e status do plano
            if (request.PlanoId == 0)
            {
                resultado.Errors.Add(new ValidationFailure(nameof(request.PlanoId), "PlanoId é obrigatório e deve ser maior que zero."));
            }
            else
            {
                var plano = await _planoReadOnlyRepositorio.ObterPorIdAsync(request.PlanoId);
                if (plano == null || !plano.Ativo)
                {
                    resultado.Errors.Add(new ValidationFailure(nameof(request.PlanoId), "Plano informado não existe ou está inativo."));
                }
            }

            // Se houver erros, lança exceção
            if (!resultado.IsValid)
            {
                var mensagensErro = resultado.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErroDeValidacao(mensagensErro);
            }
        }
    }
}
