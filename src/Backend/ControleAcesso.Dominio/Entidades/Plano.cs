namespace ControleAcesso.Dominio.Entidades;

public class Plano : EntidadeBase
{
    public string Nome { get; set; }

    public List<AreaPermitida> AreasPermitidas { get; set; } = new List<AreaPermitida>();

    public ICollection<Socio> Socios { get; set; } = new List<Socio>();
}