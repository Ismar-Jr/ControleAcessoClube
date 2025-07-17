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
    /// <summary>
    /// Caso de uso para registro de uma nova área.
    /// </summary>
    public class RegistroAreaUseCase : IRegistroAreaUseCase
    {
        private readonly IAreaWriteOnlyRepositorio _repositorioEscrita;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
        private readonly IMapper _mapper;
        private readonly ValidacaoRegistroArea _validador;

        /// <summary>
        /// Construtor com injeção das dependências.
        /// </summary>
        public RegistroAreaUseCase(
            IAreaWriteOnlyRepositorio repositorioEscrita,
            IMapper mapper,
            IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _repositorioEscrita = repositorioEscrita;
            _mapper = mapper;
            _unidadeDeTrabalho = unidadeDeTrabalho;
            _validador = new ValidacaoRegistroArea();
        }

        /// <summary>
        /// Executa o registro da área a partir da requisição.
        /// </summary>
        /// <param name="request">Dados da área a serem cadastrados.</param>
        /// <returns>Resposta com dados da área cadastrada.</returns>
        public async Task<RespostaRegistroAreaJson> Execute(RequisicaoRegistroAreaJson request)
        {
            await Validar(request);

            var area = _mapper.Map<Dominio.Entidades.Area>(request);

            await _repositorioEscrita.Add(area);
            await _unidadeDeTrabalho.Commit();

            return new RespostaRegistroAreaJson
            {
                Id = area.Id,
                Nome = area.Nome
            };
        }

        /// <summary>
        /// Valida os dados da requisição para registro da área.
        /// </summary>
        /// <param name="request">Dados da requisição.</param>
        /// <exception cref="ErroDeValidacao">Disparada quando dados inválidos são encontrados.</exception>
        private Task Validar(RequisicaoRegistroAreaJson request)
        {
            var resultado = _validador.Validate(request);

            if (!resultado.IsValid)
            {
                var mensagensErro = resultado.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErroDeValidacao(mensagensErro);
            }

            return Task.CompletedTask;
        }
    }
}
