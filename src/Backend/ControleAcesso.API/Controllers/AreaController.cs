using ControleAcesso.Aplicacao.UseCases.Area.Registro;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace ControleAcesso.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AreaController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(RespostaRegistroAreaJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] IRegistroAreaUseCase useCase,
        [FromBody] RequisicaoRegistroAreaJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }
}