namespace ControleAcesso.Dominio.Repositorios.AreaPermitida.Registro;

public interface IAreaPermitidaWriteOnlyRepositorio
{
    /// <summary>
    /// Adiciona uma nova relação entre plano e área permitida.
    /// </summary>
    public Task AdicionarAsync(Entidades.AreaPermitida areaPermitida);

    public Task RemoverPorPlanoIdAsync(long planoId);

}