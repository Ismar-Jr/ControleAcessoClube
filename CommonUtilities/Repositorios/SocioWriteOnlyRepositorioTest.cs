using ControleAcesso.Dominio.Repositorios.Socio;
using Moq;

namespace CommonUtilities.Repositorios;

public class SocioWriteOnlyRepositorioTest
{
    /// <summary>
    /// Retorna uma instância simulada de IUserWriteOnlyRepository.
    /// </summary>
    public static ISocioWriteOnlyRepositorio Build()
    {
        var mock = new Mock<ISocioWriteOnlyRepositorio>();
        return mock.Object;
    }
}