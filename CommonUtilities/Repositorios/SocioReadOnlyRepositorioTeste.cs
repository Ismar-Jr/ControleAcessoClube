using ControleAcesso.Dominio.Repositorios.Socio;
using Moq;

namespace CommonUtilities.Repositorios;

public class SocioReadOnlyRepositorioTeste
{
    private readonly Mock<ISocioReadOnlyRepositorio> _repository;
    
    public SocioReadOnlyRepositorioTeste() => _repository = new Mock<ISocioReadOnlyRepositorio>();

    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }
    public ISocioReadOnlyRepositorio Build() => _repository.Object;
}