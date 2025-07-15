using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Repositorios.Socio;
using Microsoft.EntityFrameworkCore;

namespace ControleAcesso.Infraestrutura.DataAccess.Repositorios;

/// <summary>
/// Implementação do repositório de socios, responsável por persistir e consultar dados no banco.
/// Implementa tanto a interface de leitura quanto a de escrita.
/// </summary>
public class SocioRepositorio : ISocioReadOnlyRepositorio, ISocioWriteOnlyRepositorio
{
    // Referência para o contexto do banco de dados (EF Core), injetado via construtor.
    private readonly ControleAcessoDbContext _dbContext;

    /// <summary>
    /// Construtor que injeta o DbContext da aplicação.
    /// </summary>
    /// <param name="dbContext">Instância do contexto do banco de dados.</param>
    public SocioRepositorio(ControleAcessoDbContext dbContext) => _dbContext = dbContext;

    /// <summary>
    /// Adiciona um novo usuário ao banco de dados.
    /// A operação ainda não é persistida no banco até que SaveChangesAsync seja chamado (normalmente pela Unit of Work).
    /// </summary>
    /// <param name="socio">Entidade de usuário a ser adicionada.</param>
    public async Task Add(Socio socio)
    {
        await _dbContext.Socios.AddAsync(socio);
    } 

    /// <summary>
    /// Verifica se existe um usuário ativo com o CPF informado.
    /// Retorna true se o CPF já estiver em uso por um usuário com status ativo.
    /// </summary>
    /// <param name="cpf">CPF a ser verificado.</param>
    /// <returns>True se existir um usuário ativo com o CPF informado, false caso contrário.</returns>
    public async Task<bool> ExistActiveUserWithCpf(string cpf) =>
        await _dbContext.Socios.AnyAsync(socio => socio.Cpf.Equals(cpf) && socio.Ativo);
}