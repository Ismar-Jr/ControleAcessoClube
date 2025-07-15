namespace ControleAcesso.Dominio.Repositorios.Plano;

public interface IPlanoWriteOnlyRepositorio
{
    public Task Add(Entidades.Plano plano);
}