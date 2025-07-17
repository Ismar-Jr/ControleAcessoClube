using ControleAcesso.Aplicacao.UseCases.Socio.Atualizacao;
using ControleAcesso.Aplicacao.UseCases.Socio.Registro;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Dominio.Repositorios.Socio;
using ControleAcesso.Excecoes.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;

namespace ControleAcesso.API.Controllers;

[Route("[controller]")]
[ApiController]
public class SocioController : ControllerBase
{
    /// <summary>
    /// Registra um novo sócio.
    /// </summary>
    /// <param name="useCase">Caso de uso para registro de sócio.</param>
    /// <param name="request">Dados do sócio a ser registrado.</param>
    /// <returns>Dados do sócio registrado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RespostaRegistroSocioJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegistrarAsync(
        [FromServices] IRegistroSocioUseCase useCase,
        [FromBody] RequisicaoRegistroSocioJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    /// <summary>
    /// Retorna um sócio específico pelo ID.
    /// </summary>
    /// <param name="readOnlyRepositorio">Repositório de leitura de sócios.</param>
    /// <param name="id">ID do sócio.</param>
    /// <returns>Dados do sócio.</returns>
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
    /// <param name="readOnlyRepositorio">Repositório de leitura de sócios.</param>
    /// <returns>Lista de sócios ativos.</returns>
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
    /// Atualiza os dados de um sócio existente.
    /// </summary>
    /// <param name="id">ID do sócio</param>
    /// <param name="request">Dados atualizados</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(long id, 
        [FromBody] RequisicaoRegistroSocioJson request,
        [FromServices] IAtualizarSocioUseCase _atualizarSocioUseCase)
    {
        try
        {
            await _atualizarSocioUseCase.Execute(id, request);
            return NoContent(); // ou Ok() se preferir
        }
        catch (ErroDeValidacao ex)
        {
            return BadRequest(new { erros = ex.ErrorMessages });
        }
    }

    /// <summary>
    /// Desativa um sócio (soft delete).
    /// </summary>
    /// <param name="readOnlyRepositorio">Repositório de leitura de sócios.</param>
    /// <param name="writeOnlyRepositorio">Repositório de escrita de sócios.</param>
    /// <param name="id">ID do sócio a ser desativado.</param>
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
