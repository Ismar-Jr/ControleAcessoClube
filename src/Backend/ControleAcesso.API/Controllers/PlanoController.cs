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
    [HttpPost]
    [ProducesResponseType(typeof(RespostaRegistroPlanoJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegistrarPlanoAsync(
        [FromServices] IRegistroPlanoUseCase useCase,
        [FromBody] RequisicaoRegistroPlanoJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

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