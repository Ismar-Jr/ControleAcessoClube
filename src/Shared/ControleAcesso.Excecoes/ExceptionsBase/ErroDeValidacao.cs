namespace ControleAcesso.Excecoes.ExceptionsBase;

public class ErroDeValidacao : ErroControleAcesso
{
    public IList<string> ErrorMessages { get; set; }
    
    public ErroDeValidacao(IList<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}