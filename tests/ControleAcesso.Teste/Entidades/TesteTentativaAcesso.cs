namespace ControleAcesso.Teste.Entidades;
public class TesteTentativaAcesso
{
    public long SocioId { get; set; }
    public long AreaId { get; set; }
    public bool Esperado { get; set; }  // Acesso permitido ou negado
    public bool GeraException { get; set; } // Se espera que o caso de uso lance exceção
}
