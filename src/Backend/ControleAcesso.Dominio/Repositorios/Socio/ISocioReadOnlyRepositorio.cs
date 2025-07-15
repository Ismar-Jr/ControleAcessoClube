namespace ControleAcesso.Dominio.Repositorios.Socio;

public interface ISocioReadOnlyRepositorio
{
    public Task<bool> ExistActiveUserWithCpf(string cpf);
}