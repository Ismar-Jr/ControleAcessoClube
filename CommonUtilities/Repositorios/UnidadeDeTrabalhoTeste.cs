using ControleAcesso.Dominio.Repositorios;
using Moq;

namespace CommonUtilities.Repositorios;

public class UnidadeDeTrabalhoTeste
{
    // <summary>
    /// Cria e retorna uma instância mockada de IUnitOfWork.
    /// Ideal para testes de casos de uso sem acesso real ao banco de dados.
    /// </summary>
    public static IUnidadeDeTrabalho Build()
    {
        var mock = new Mock<IUnidadeDeTrabalho>();
        return mock.Object;
    }
}