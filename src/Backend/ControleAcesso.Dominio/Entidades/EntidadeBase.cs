namespace ControleAcesso.Dominio.Entidades;

public class EntidadeBase 
{
    public long Id { get; set; }
    
    public DateTime CriadoEm { get; set; } = DateTime.Now;
    
    public bool Ativo { get; set; } = true;
}