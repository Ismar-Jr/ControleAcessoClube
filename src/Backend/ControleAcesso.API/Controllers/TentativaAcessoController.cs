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

    /// <summary>
    /// Construtor que recebe o caso de uso para registrar tentativa de acesso.
    /// </summary>
    /// <param name="useCase">Caso de uso para registro de tentativa de acesso.</param>
    public TentativaAcessoController(IRegistrarTentativaAcessoUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>
    /// Registra uma tentativa de acesso.
    /// </summary>
    /// <param name="request">Dados da tentativa de acesso.</param>
    /// <returns>Resultado da tentativa de acesso.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RespostaTentativaAcessoJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegistrarTentativaAsync([FromBody] RequisicaoTentativaAcessoJson request)
    {
        var resultado = await _useCase.Execute(request);
        return Created(string.Empty, resultado);
    }
}