using ControleAcesso.Aplicacao.UseCases.Area.Registro;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Repositorios.Area;
using Microsoft.AspNetCore.Mvc;

namespace ControleAcesso.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AreaController : ControllerBase
{
    /// <summary>
    /// Registra uma nova área.
    /// </summary>
    /// <param name="useCase">Caso de uso para registro da área.</param>
    /// <param name="request">Dados da área a ser registrada.</param>
    /// <returns>Dados da área registrada.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RespostaRegistroAreaJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegistrarAsync(
        [FromServices] IRegistroAreaUseCase useCase,
        [FromBody] RequisicaoRegistroAreaJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    /// <summary>
    /// Obtém uma área pelo ID.
    /// </summary>
    /// <param name="readOnlyRepositorio">Repositório de leitura de áreas.</param>
    /// <param name="id">ID da área.</param>
    /// <returns>Dados da área, se encontrada.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RespostaRegistroAreaJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorIdAsync(
        [FromServices] IAreaReadOnlyRepositorio readOnlyRepositorio,
        [FromRoute] long id)
    {
        var area = await readOnlyRepositorio.ObterPorIdAsync(id);
        if (area == null || !area.Ativo)
            return NotFound();

        var resposta = new RespostaRegistroAreaJson
        {
            Id = area.Id,
            Nome = area.Nome
        };

        return Ok(resposta);
    }

    /// <summary>
    /// Lista todas as áreas ativas.
    /// </summary>
    /// <param name="readOnlyRepositorio">Repositório de leitura de áreas.</param>
    /// <returns>Lista de áreas ativas.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RespostaRegistroAreaJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarAtivosAsync(
        [FromServices] IAreaReadOnlyRepositorio readOnlyRepositorio)
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
    /// Atualiza uma área existente.
    /// </summary>
    /// <param name="writeOnlyRepositorio">Repositório de escrita.</param>
    /// <param name="readOnlyRepositorio">Repositório de leitura.</param>
    /// <param name="id">ID da área a ser atualizada.</param>
    /// <param name="request">Novos dados da área.</param>
    /// <returns>Resposta HTTP.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarAsync(
        [FromServices] IAreaWriteOnlyRepositorio writeOnlyRepositorio,
        [FromServices] IAreaReadOnlyRepositorio readOnlyRepositorio,
        [FromRoute] long id,
        [FromBody] RequisicaoRegistroAreaJson request)
    {
        var area = await readOnlyRepositorio.ObterPorIdAsync(id);
        if (area == null || !area.Ativo)
            return NotFound();

        area.Nome = request.Nome;

        await writeOnlyRepositorio.AtualizarAsync(area);
        return NoContent();
    }

    /// <summary>
    /// Desativa uma área.
    /// </summary>
    /// <param name="writeOnlyRepositorio">Repositório de escrita.</param>
    /// <param name="readOnlyRepositorio">Repositório de leitura.</param>
    /// <param name="id">ID da área a ser desativada.</param>
    /// <returns>Resposta HTTP.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DesativarAsync(
        [FromServices] IAreaWriteOnlyRepositorio writeOnlyRepositorio,
        [FromServices] IAreaReadOnlyRepositorio readOnlyRepositorio,
        [FromRoute] long id)
    {
        var area = await readOnlyRepositorio.ObterPorIdAsync(id);
        if (area == null || !area.Ativo)
            return NotFound();

        area.Ativo = false;
        await writeOnlyRepositorio.AtualizarAsync(area);
        return NoContent();
    }
}
