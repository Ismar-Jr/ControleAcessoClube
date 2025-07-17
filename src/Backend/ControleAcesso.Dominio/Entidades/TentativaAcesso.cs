using ControleAcesso.Dominio.Enums;

namespace ControleAcesso.Dominio.Entidades;

public class TentativaAcesso 
{
    public int Id { get; set; }
    public long SocioId { get; set; }
    public Socio Socio { get; set; }
    
    public long AreaId { get; set; }
    public Area Area { get; set; }
    
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
    
    public ResultadoAcesso Resultado { get; set; }
}