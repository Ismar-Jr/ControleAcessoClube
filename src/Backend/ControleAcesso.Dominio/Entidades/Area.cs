namespace ControleAcesso.Dominio.Entidades;

public class Area : EntidadeBase
{
    public string Nome { get; set; }

    public ICollection<AreaPermitida> PlanosQuePermitem { get; set; } = new List<AreaPermitida>();

    public ICollection<TentativaAcesso> TentativasDeAcesso { get; set; } = new List<TentativaAcesso>();
}