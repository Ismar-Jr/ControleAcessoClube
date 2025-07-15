namespace ControleAcesso.Dominio.Entidades;

public class AreaPermitida
{
    public int PlanoId { get; set; }
    public Plano Plano { get; set; }
    
    public int AreaId { get; set; }
    public Area Area { get; set; }
}