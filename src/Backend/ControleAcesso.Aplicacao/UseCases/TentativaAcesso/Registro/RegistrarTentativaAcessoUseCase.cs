using AutoMapper;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Enums;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.Area;
using ControleAcesso.Dominio.Repositorios.Socio;
using ControleAcesso.Dominio.Repositorios.TentativaAcesso;
using ControleAcesso.Excecoes.ExceptionsBase;

namespace ControleAcesso.Aplicacao.UseCases.TentativaAcesso.Registro
{
    /// <summary>
    /// Caso de uso para registrar tentativas de acesso de sócios a áreas.
    /// </summary>
    public class RegistrarTentativaAcessoUseCase : IRegistrarTentativaAcessoUseCase
    {
        private readonly ISocioReadOnlyRepositorio _repositorioSocio;
        private readonly IAreaReadOnlyRepositorio _repositorioArea;
        private readonly ITentativaAcessoWriteOnlyRepositorio _repositorioTentativa;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        /// <summary>
        /// Inicializa uma nova instância do caso de uso de registro de tentativa de acesso.
        /// </summary>
        public RegistrarTentativaAcessoUseCase(ISocioReadOnlyRepositorio repositorioSocio,
            IAreaReadOnlyRepositorio repositorioArea,
            ITentativaAcessoWriteOnlyRepositorio repositorioTentativa,
            IMapper mockMapperObject,
            IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _repositorioSocio = repositorioSocio;
            _repositorioArea = repositorioArea;
            _repositorioTentativa = repositorioTentativa;
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }

        /// <summary>
        /// Executa o registro da tentativa de acesso, validando permissões e registrando o resultado.
        /// </summary>
        /// <param name="request">Dados da requisição de tentativa de acesso.</param>
        /// <returns>Resposta contendo o resultado da tentativa.</returns>
        /// <exception cref="ErroDeValidacao">Lançada em caso de dados inválidos ou inexistentes.</exception>
        public async Task<RespostaTentativaAcessoJson> Execute(RequisicaoTentativaAcessoJson request)
        {
            var socio = await _repositorioSocio.ObterPorIdAsync(request.SocioId)
                        ?? throw new ErroDeValidacao(new List<string> { "Sócio não encontrado." });

            if (!socio.Ativo)
                throw new ErroDeValidacao(new List<string> { "Sócio está inativo." });

            var area = await _repositorioArea.ObterPorIdAsync(request.AreaId)
                       ?? throw new ErroDeValidacao(new List<string> { "Área não encontrada." });

            if (!area.Ativo)
                throw new ErroDeValidacao(new List<string> { "Área está inativa." });

            var plano = socio.Plano;
            if (plano == null)
                throw new ErroDeValidacao(new List<string> { "Sócio não possui plano associado." });

            if (!plano.Ativo)
                throw new ErroDeValidacao(new List<string> { "Plano do sócio está inativo." });

            var autorizado = plano.AreasPermitidas
                .Any(ap => ap.AreaId == area.Id);

            var mensagemResultado = autorizado
                ? "Acesso autorizado."
                : "Acesso negado: O sócio não possui permissão para acessar esta área.";

            var tentativa = new Dominio.Entidades.TentativaAcesso
            {
                SocioId = socio.Id,
                AreaId = area.Id,
                DataHora = DateTime.UtcNow,
                Resultado = autorizado ? ResultadoAcesso.Autorizado : ResultadoAcesso.Negado
            };

            await _repositorioTentativa.Add(tentativa);
            await _unidadeDeTrabalho.Commit();

            return new RespostaTentativaAcessoJson
            {
                SocioId = socio.Id,
                AreaId = area.Id,
                DataHora = tentativa.DataHora,
                Autorizado = autorizado,
                MensagemResultado = mensagemResultado
            };
        }
    }
}
