using ControleAcesso.Aplicacao.UseCases.Plano.Registro;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Repositorios.Plano;
using Microsoft.AspNetCore.Mvc;

namespace ControleAcesso.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PlanoController : ControllerBase
{
    /// <summary>
    /// Registra um novo plano no sistema.
    /// </summary>
    /// <param name="useCase">Caso de uso responsável pelo registro do plano.</param>
    /// <param name="request">Dados do plano a ser registrado.</param>
    /// <returns>Retorna o plano criado com status 201.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RespostaRegistroPlanoJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegistrarAsync(
        [FromServices] IRegistroPlanoUseCase useCase,
        [FromBody] RequisicaoRegistroPlanoJson request)
    {
        var resposta = await useCase.Execute(request);
        return Created(string.Empty, resposta);
    }

    /// <summary>
    /// Atualiza as informações de um plano existente.
    /// </summary>
    /// <param name="readOnlyRepositorio">Repositório somente leitura.</param>
    /// <param name="writeOnlyRepositorio">Repositório para escrita.</param>
    /// <param name="id">ID do plano a ser atualizado.</param>
    /// <param name="request">Dados atualizados do plano.</param>
    /// <returns>Retorna status 204 se a atualização for bem-sucedida.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AtualizarAsync(
        [FromServices] IPlanoReadOnlyRepositorio readOnlyRepositorio,
        [FromServices] IPlanoWriteOnlyRepositorio writeOnlyRepositorio,
        [FromRoute] long id,
        [FromBody] RequisicaoRegistroPlanoJson request)
    {
        var plano = await readOnlyRepositorio.ObterPorIdAsync(id);
        plano.Nome = request.Nome;
        await writeOnlyRepositorio.AtualizarAsync(plano);
        return NoContent();
    }
    /// <summary>
    /// Lista todos os planos ativos.
    /// </summary>
    /// <param name="readOnlyRepositorio">Repositório de leitura de planos.</param>
    /// <returns>Lista de planos ativos.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RespostaRegistroAreaJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarAtivosAsync(
        [FromServices] IPlanoReadOnlyRepositorio readOnlyRepositorio)
    {
        var areas = await readOnlyRepositorio.ListarAtivosAsync();

        var resposta = areas.Select(a => new RespostaRegistroAreaJson
        {
            Id = a.Id,
            Nome = a.Nome
        });

        return Ok(resposta);
    }


    /// <summary>
    /// Desativa um plano existente.
    /// </summary>
    /// <param name="readOnlyRepositorio">Repositório somente leitura.</param>
    /// <param name="writeOnlyRepositorio">Repositório para escrita.</param>
    /// <param name="id">ID do plano a ser desativado.</param>
    /// <returns>Retorna status 204 se a desativação for bem-sucedida.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DesativarAsync(
        [FromServices] IPlanoReadOnlyRepositorio readOnlyRepositorio,
        [FromServices] IPlanoWriteOnlyRepositorio writeOnlyRepositorio,
        [FromRoute] long id)
    {
        var plano = await readOnlyRepositorio.ObterPorIdAsync(id);
        plano.Ativo = false;
        await writeOnlyRepositorio.AtualizarAsync(plano);
        return NoContent();
    }
}
