using ControleAcesso.Aplicacao.UseCases.Socio.Registro;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Repositorios.Socio;
using Microsoft.AspNetCore.Mvc;

namespace ControleAcesso.API.Controllers;

[Route("[controller]")]
[ApiController]
public class SocioController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(RespostaRegistroSocioJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegistrarSocioAsync(
        [FromServices] IRegistroSocioUseCase useCase,
        [FromBody] RequisicaoRegistroSocioJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    /// <summary>
    /// Retorna um sócio específico pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RespostaRegistroSocioJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorIdAsync(
        [FromServices] ISocioReadOnlyRepositorio readOnlyRepositorio,
        [FromRoute] long id)
    {
        var socio = await readOnlyRepositorio.ObterPorIdAsync(id);
        if (socio == null || !socio.Ativo)
            return NotFound();

        var resposta = new RespostaRegistroSocioJson
        {
            Id = socio.Id,
            Nome = socio.Nome,
            Cpf = socio.Cpf,
            Email = socio.Email
        };

        return Ok(resposta);
    }

    /// <summary>
    /// Retorna todos os sócios ativos.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RespostaRegistroSocioJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarTodosAtivosAsync(
        [FromServices] ISocioReadOnlyRepositorio readOnlyRepositorio)
    {
        var socios = await readOnlyRepositorio.ListarAtivosAsync();

        var resposta = socios.Select(s => new RespostaRegistroSocioJson
        {
            Id = s.Id,
            Nome = s.Nome,
            Cpf = s.Cpf,
            Email = s.Email
        });

        return Ok(resposta);
    }

    /// <summary>
    /// Atualiza dados de um sócio.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarAsync(
        [FromServices] ISocioReadOnlyRepositorio readOnlyRepositorio,
        [FromServices] ISocioWriteOnlyRepositorio writeOnlyRepositorio,
        [FromRoute] long id,
        [FromBody] RequisicaoRegistroSocioJson request)
    {
        var socio = await readOnlyRepositorio.ObterPorIdAsync(id);
        if (socio == null || !socio.Ativo)
            return NotFound();

        socio.Nome = request.Nome;
        socio.Cpf = request.Cpf;
        socio.Email = request.Email;

        await writeOnlyRepositorio.AtualizarAsync(socio);
        return NoContent();
    }

    /// <summary>
    /// Desativa um sócio (soft delete).
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DesativarAsync(
        [FromServices] ISocioReadOnlyRepositorio readOnlyRepositorio,
        [FromServices] ISocioWriteOnlyRepositorio writeOnlyRepositorio,
        [FromRoute] long id)
    {
        var socio = await readOnlyRepositorio.ObterPorIdAsync(id);
        if (socio == null || !socio.Ativo)
            return NotFound();

        socio.Ativo = false;
        await writeOnlyRepositorio.AtualizarAsync(socio);
        return NoContent();
    }
}
