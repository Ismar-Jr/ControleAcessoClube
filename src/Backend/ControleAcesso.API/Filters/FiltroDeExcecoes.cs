using ControleAcesso.Comunicacao.Respostas;
using ControleAcesso.Excecoes.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ControleAcesso.API.Filters;

public class FiltroDeExcecoes : IExceptionFilter
{
    /// <summary>
    /// Executado automaticamente quando ocorre uma exceção durante o processamento de uma requisição.
    /// </summary>
    /// <param name="context">Contexto contendo a exceção e dados da requisição.</param>
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ErroControleAcesso)
            HandleProjectException(context);
        //else
            //HandleUnknownException(context);
    }

    /// <summary>
    /// Trata exceções customizadas do projeto, atualmente focado em erros de validação.
    /// Retorna BadRequest (400) com mensagens detalhadas.
    /// </summary>
    private void HandleProjectException(ExceptionContext context)
    {
        if (context.Exception is ErroDeValidacao validationException)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(new RespostaErroJson(validationException.ErrorMessages));
        }
    }

    /// <summary>
    /// Trata exceções inesperadas, retornando erro genérico 500 sem expor detalhes sensíveis.
    /// </summary>
    /*private void HandleUnknownException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var response = new RespostaErroJson(new List<string> { "Erro inesperado. Tente novamente mais tarde." });
        context.Result = new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
    }*/
}