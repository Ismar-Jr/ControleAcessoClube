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
    [HttpPost]
    [ProducesResponseType(typeof(RespostaRegistroAreaJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterAsync(
        [FromServices] IRegistroAreaUseCase useCase,
        [FromBody] RequisicaoRegistroAreaJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RespostaRegistroAreaJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(
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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RespostaRegistroAreaJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarTodosAtivosAsync(
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
