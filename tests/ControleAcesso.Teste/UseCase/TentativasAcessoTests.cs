using Xunit;
using Moq;
using System.Text.Json;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Dominio.Repositorios.Socio;
using ControleAcesso.Dominio.Repositorios.Area;
using ControleAcesso.Dominio.Repositorios.TentativaAcesso;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Aplicacao.UseCases.TentativaAcesso.Registro;
using AutoMapper;
using ControleAcesso.Dominio.Enums;
using ControleAcesso.Teste.Entidades;
using ControleAcesso.Excecoes.ExceptionsBase; // <-- Importa o namespace da exceção customizada
using Assert = Xunit.Assert;

namespace ControleAcesso.Teste.UseCase
{
    /// <summary>
    /// Contém os testes para o caso de uso de registro de tentativas de acesso,
    /// validando cenários diversos incluindo sucesso, falhas e exceções.
    /// </summary>
    public class TentativasAcessoTests
    {
        /// <summary>
        /// Testa diferentes cenários de tentativa de acesso com dados parametrizados,
        /// validando se o resultado é autorizado ou se uma exceção é lançada quando esperado.
        /// </summary>
        /// <param name="caso">Cenário de teste contendo dados de entrada e expectativas.</param>
        [Xunit.Theory]
        [MemberData(nameof(CarregarCenarios))]
        public async Task ValidarTentativaAcesso(TesteTentativaAcesso caso)
        {
            var socios = CarregarJson<List<Socio>>(Path.Combine("..", "..", "..", "dados", "socios.json"));
            var planos = CarregarJson<List<Plano>>(Path.Combine("..", "..", "..", "dados", "planos.json"));
            var areas = CarregarJson<List<Area>>(Path.Combine("..", "..", "..", "dados", "areas.json"));
            var areasPermitidas = CarregarJson<List<AreaPermitida>>(Path.Combine("..", "..", "..", "dados", "areas_permitidas.json"));

            // Pode retornar null se não encontrar
            var socio = socios.FirstOrDefault(s => s.Id == caso.SocioId);
            if (socio != null)
            {
                socio.Plano = planos.FirstOrDefault(p => p.Id == socio.PlanoId);
                if (socio.Plano != null)
                {
                    socio.Plano.AreasPermitidas = areasPermitidas
                        .Where(ap => ap.PlanoId == socio.PlanoId)
                        .ToList();
                }
            }

            var area = areas.FirstOrDefault(a => a.Id == caso.AreaId);

            var mockSocioRepo = new Mock<ISocioReadOnlyRepositorio>();
            mockSocioRepo.Setup(r => r.ObterPorIdAsync(caso.SocioId)).ReturnsAsync(socio);

            var mockAreaRepo = new Mock<IAreaReadOnlyRepositorio>();
            mockAreaRepo.Setup(r => r.ObterPorIdAsync(caso.AreaId)).ReturnsAsync(area);

            var mockTentativaRepo = new Mock<ITentativaAcessoWriteOnlyRepositorio>();
            mockTentativaRepo.Setup(r => r.Add(It.IsAny<TentativaAcesso>())).Returns(Task.CompletedTask);

            var mockUnitOfWork = new Mock<IUnidadeDeTrabalho>();
            mockUnitOfWork.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var mockMapper = new Mock<IMapper>();

            var useCase = new RegistrarTentativaAcessoUseCase(
                mockSocioRepo.Object,
                mockAreaRepo.Object,
                mockTentativaRepo.Object,
                mockMapper.Object,
                mockUnitOfWork.Object
            );

            var requisicao = new RequisicaoTentativaAcessoJson
            {
                SocioId = caso.SocioId,
                AreaId = caso.AreaId
            };

            if (caso.GeraException)
            {
                // Se espera erro, valida se realmente lança ErroDeValidacao
                await Assert.ThrowsAsync<ErroDeValidacao>(async () => await useCase.Execute(requisicao));
                return;
            }

            // Caso normal, executa e verifica autorização
            var resposta = await useCase.Execute(requisicao);
            Assert.Equal(caso.Esperado, resposta.Autorizado);

            var tentativa = new TentativaAcesso
            {
                SocioId = resposta.SocioId,
                AreaId = resposta.AreaId,
                DataHora = resposta.DataHora,
                Resultado = resposta.Autorizado ? ResultadoAcesso.Autorizado : ResultadoAcesso.Negado
            };

            await SalvarTentativaNoArquivoAsync(tentativa);
        }

        /// <summary>
        /// Carrega cenários de teste do arquivo JSON, preparando-os para uso no atributo Theory do Xunit.
        /// </summary>
        /// <returns>Sequência de arrays de objetos representando os parâmetros dos testes.</returns>
        public static IEnumerable<object[]> CarregarCenarios()
        {
            var caminho = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "dados", "resultado_esperado.json");
            var caminhoNormalizado = Path.GetFullPath(caminho);
            var json = File.ReadAllText(caminhoNormalizado);
            var casos = JsonSerializer.Deserialize<List<TesteTentativaAcesso>>(json)!;

            // Retorna cada cenário como um parâmetro para o teste
            return casos.Select(c => new object[] { c });
        }

        /// <summary>
        /// Carrega um arquivo JSON tipado a partir de um caminho relativo ao diretório base do executável.
        /// </summary>
        /// <typeparam name="T">Tipo para o qual o JSON será desserializado.</typeparam>
        /// <param name="caminhoRelativo">Caminho relativo ao diretório base.</param>
        /// <returns>Objeto desserializado do JSON.</returns>
        private static T CarregarJson<T>(string caminhoRelativo)
        {
            var caminhoCompleto = Path.Combine(AppContext.BaseDirectory, caminhoRelativo);
            var caminhoNormalizado = Path.GetFullPath(caminhoCompleto);
            var json = File.ReadAllText(caminhoNormalizado);
            return JsonSerializer.Deserialize<T>(json)!;
        }

        /// <summary>
        /// Salva a tentativa de acesso em arquivo JSON, adicionando-a ao conteúdo existente.
        /// </summary>
        /// <param name="tentativa">Objeto da tentativa de acesso a ser salva.</param>
        /// <returns>Tarefa assíncrona representando a operação de escrita em arquivo.</returns>
        private static async Task SalvarTentativaNoArquivoAsync(TentativaAcesso tentativa)
        {
            var caminho = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "dados", "tentativas_acesso.json");
            var caminhoNormalizado = Path.GetFullPath(caminho);

            List<TentativaAcesso> tentativasExistentes = new();

            if (File.Exists(caminhoNormalizado))
            {
                var jsonExistente = await File.ReadAllTextAsync(caminhoNormalizado);
                if (!string.IsNullOrWhiteSpace(jsonExistente))
                {
                    tentativasExistentes = JsonSerializer.Deserialize<List<TentativaAcesso>>(jsonExistente)
                        ?? new List<TentativaAcesso>();
                }
            }

            tentativasExistentes.Add(tentativa);

            var jsonAtualizado = JsonSerializer.Serialize(tentativasExistentes, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(caminhoNormalizado, jsonAtualizado);
        }
    }
}
