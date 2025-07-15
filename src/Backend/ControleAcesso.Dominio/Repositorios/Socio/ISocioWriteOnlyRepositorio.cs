namespace ControleAcesso.Dominio.Repositorios.Socio;

public interface ISocioWriteOnlyRepositorio
{
    public Task Add(Entidades.Socio socio);
}