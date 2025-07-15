namespace ControleAcesso.Dominio.Entidades;

public class Socio : EntidadeBase
{
    public string? Nome { get; set; }

    public string? Cpf { get; set; }
    
    public string? Email { get; set; }
    
    public string? Senha { get; set; }
    
    public int PlanoId { get; set; }

    public Plano Plano { get; set; }
}