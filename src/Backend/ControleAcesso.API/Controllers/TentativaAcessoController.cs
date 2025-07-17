using ControleAcesso.Aplicacao.UseCases.TentativaAcesso.Registro;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace ControleAcesso.API.Controllers;

[Route("[controller]")]
[ApiController]
public class TentativaAcessoController : ControllerBase
{
    private readonly IRegistrarTentativaAcessoUseCase _useCase;

    public TentativaAcessoController(IRegistrarTentativaAcessoUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(RespostaTentativaAcessoJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegistrarTentativaAsync([FromBody] RequisicaoTentativaAcessoJson request)
    {
        var resultado = await _useCase.Execute(request);
        return Created(string.Empty, resultado);
    }
}