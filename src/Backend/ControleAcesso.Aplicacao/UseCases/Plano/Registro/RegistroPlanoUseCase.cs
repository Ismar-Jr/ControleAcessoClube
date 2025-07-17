using AutoMapper;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.Area;
using ControleAcesso.Excecoes.ExceptionsBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleAcesso.Dominio.Repositorios.Plano;

namespace ControleAcesso.Aplicacao.UseCases.Plano.Registro
{
    /// <summary>
    /// Caso de uso para registro de um novo plano.
    /// </summary>
    public class RegistroPlanoUseCase : IRegistroPlanoUseCase
    {
        private readonly IPlanoWriteOnlyRepositorio _repositorioEscrita;
        private readonly IAreaReadOnlyRepositorio _areaReadOnlyRepositorio;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
        
        /// <summary>
        /// Construtor com injeção das dependências.
        /// </summary>
        public RegistroPlanoUseCase(
            IPlanoWriteOnlyRepositorio repositorioEscrita,
            IAreaReadOnlyRepositorio areaReadOnlyRepositorio,
            IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _repositorioEscrita = repositorioEscrita;
            _areaReadOnlyRepositorio = areaReadOnlyRepositorio;
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }
        
        /// <summary>
        /// Executa o registro do plano a partir da requisição.
        /// </summary>
        /// <param name="request">Dados do plano a serem cadastrados.</param>
        /// <returns>Resposta com dados do plano cadastrado.</returns>
        public async Task<RespostaRegistroPlanoJson> Execute(RequisicaoRegistroPlanoJson request)
        {
            await Validar(request);
            
            var plano = new Dominio.Entidades.Plano
            {
                Nome = request.Nome!,
                AreasPermitidas = request.IdsAreasPermitidas
                    .Select(areaId => new Dominio.Entidades.AreaPermitida { AreaId = areaId })
                    .ToList()
            };

            await _repositorioEscrita.Add(plano);
            await _unidadeDeTrabalho.Commit();

            return new RespostaRegistroPlanoJson
            {
                Nome = plano.Nome,
            };
        }
        
        /// <summary>
        /// Valida os dados da requisição para registro do plano,
        /// incluindo verificação se as áreas existem e estão ativas.
        /// </summary>
        /// <param name="request">Dados da requisição.</param>
        /// <exception cref="ErroDeValidacao">Disparada quando dados inválidos são encontrados.</exception>
        private async Task Validar(RequisicaoRegistroPlanoJson request)
        {
            var validador = new ValidacaoRegistroPlano();
            var resultado = validador.Validate(request);
            
            if (!resultado.IsValid)
            {
                var mensagensErro = resultado.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErroDeValidacao(mensagensErro);
            }
            
            // Verifica se todas as áreas enviadas existem e estão ativas
            var areasAtivas = await _areaReadOnlyRepositorio.ListarAtivosAsync();
            var idsAtivos = areasAtivas.Select(a => a.Id).ToHashSet();

            var idsInvalidos = request.IdsAreasPermitidas
                .Where(id => !idsAtivos.Contains(id))
                .ToList();

            if (idsInvalidos.Any())
            {
                throw new ErroDeValidacao(new List<string> { $"IDs de áreas inválidas ou inativas: {string.Join(", ", idsInvalidos)}" });
            }
        }
    }
}
