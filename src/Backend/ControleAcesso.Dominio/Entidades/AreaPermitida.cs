namespace ControleAcesso.Dominio.Entidades;

public class AreaPermitida : EntidadeBase
{
    public long PlanoId { get; set; }
    public Plano Plano { get; set; }
    
    public long AreaId { get; set; }
    public Area Area { get; set; }
}