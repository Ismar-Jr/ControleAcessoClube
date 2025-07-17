namespace ControleAcesso.Dominio.Repositorios.TentativaAcesso;

public interface ITentativaAcessoWriteOnlyRepositorio
{
    Task Add(Entidades.TentativaAcesso tentativa);
}