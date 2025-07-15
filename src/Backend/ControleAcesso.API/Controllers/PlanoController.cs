using ControleAcesso.Aplicacao.Socio.Registro;
using ControleAcesso.Aplicacao.UseCases.Plano.Registro;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace ControleAcesso.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PlanoController : ControllerBase
{
    /// <summary>
    /// Endpoint para registrar um novo sócio.
    /// </summary>
    /// <param name="useCase">Caso de uso injetado para execução da lógica de cadastro.</param>
    /// <param name="request">Dados do usuário enviados no corpo da requisição.</param>
    /// <returns>Retorna status 201 (Created) com os dados do novo usuário.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RespostaRegistroPlanoJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] IRegistroPlanoUseCase useCase,
        [FromBody] RequisicaoRegistroPlanoJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }
}