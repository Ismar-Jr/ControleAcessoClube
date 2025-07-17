using FluentMigrator;

namespace ControleAcesso.Infraestrutura.Migracoes.Versoes;

/// <summary>
/// Versão de migração responsável por criar a tabela "Users" no banco de dados.
/// Esta versão inicial inclui as colunas:
/// - Name: nome do usuário
/// - Email: endereço de e-mail
/// - Password: senha criptografada
/// </summary>
[Migration(DatabaseVersions.TABLE_USER, "Criar tabela para salvar as informações dos Socios")]
public class Versao001 : VersaoBase
{
    /// <summary>
    /// Método chamado ao aplicar a migração (método "Up" do FluentMigrator).
    /// Responsável por criar a estrutura da tabela "Users".
    /// </summary>
    public override void Up()
    {
        // Planos
        CreateTable("Planos")
            .WithColumn("Nome").AsString(255).NotNullable()
            .WithColumn("Descricao").AsString(1000).Nullable();
        
        // Socios
        CreateTable("Socios")
            .WithColumn("Nome").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("CPF").AsString(11).NotNullable()
            .WithColumn("Senha").AsString(2000).NotNullable()
            .WithColumn("PlanoId").AsInt64().NotNullable();

        Create.ForeignKey("FK_Socios_Planos")
            .FromTable("Socios").ForeignColumn("PlanoId")
            .ToTable("Planos").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.UniqueConstraint("UQ_Socios_Email").OnTable("Socios").Column("Email");
        Create.UniqueConstraint("UQ_Socios_CPF").OnTable("Socios").Column("CPF");

        // Areas
        CreateTable("Areas")
            .WithColumn("Nome").AsString(255).NotNullable();

        // AreaPermitidas (relacionamento Plano x Area)
        CreateTable("AreasPermitidas")
            .WithColumn("PlanoId").AsInt64().NotNullable()
            .WithColumn("AreaId").AsInt64().NotNullable();

        Create.ForeignKey("FK_AreasPermitidas_Planos")
            .FromTable("AreasPermitidas").ForeignColumn("PlanoId")
            .ToTable("Planos").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.ForeignKey("FK_AreasPermitidas_Areas")
            .FromTable("AreasPermitidas").ForeignColumn("AreaId")
            .ToTable("Areas").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        // TentativasAcesso
        Create.Table("TentativasAcesso")
            .WithColumn("Id").AsInt64().Identity().PrimaryKey()
            .WithColumn("SocioId").AsInt64().NotNullable()
            .WithColumn("AreaId").AsInt64().NotNullable()
            .WithColumn("DataHora").AsDateTime().NotNullable()
            .WithColumn("Resultado").AsInt32().NotNullable();

        Create.ForeignKey("FK_TentativasAcesso_Socios")
            .FromTable("TentativasAcesso").ForeignColumn("SocioId")
            .ToTable("Socios").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.ForeignKey("FK_TentativasAcesso_Areas")
            .FromTable("TentativasAcesso").ForeignColumn("AreaId")
            .ToTable("Areas").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }
}