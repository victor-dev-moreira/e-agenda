using eAgenda.Infraestrutura.Compartilhado.Identity;
using eAgenda.Infraestrutura.Compartilhado.Orm;
using eAgenda.Testes.Integracao.Compartilhado;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eAgenda.Testes.Integracao.Modulos.Tarefas;

[TestClass]
public abstract class RepositorioEmOrmTestsBase
{
    private static string? connectionString = null;
    protected Guid usuarioId = Guid.Empty;
    protected EAgendaDbContext dbContext = null!;


    [AssemblyInitialize]
    public static void InicializarClasse(TestContext testContext)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets(typeof(RepositorioTarefaEmOrmTests).Assembly)
            .Build();

        connectionString = configuration.GetConnectionString("SqlServer");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("A connectionStrings:SqlServer não foi configurada!");
    }

    [TestInitialize]
    public void InicializarTeste()
    {
        usuarioId = Guid.CreateVersion7();
        dbContext = CriarContexto(usuarioId);


        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        CriarUsuario(dbContext, usuarioId);
    }

    private static EAgendaDbContext CriarContexto(Guid usuarioId)
    {
        DbContextOptions<EAgendaDbContext> options = new DbContextOptionsBuilder<EAgendaDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new EAgendaDbContext(options, new ProvedorUsuarioAtualFake(usuarioId));
    }

    private static void CriarUsuario(EAgendaDbContext dbContext, Guid usuarioId)
    {
        dbContext.Users.Add(new UsuarioIdentity
        {
            Id = usuarioId,
            UserName = $"integracao-{usuarioId:N}",
            NormalizedUserName = $"INTEGRACACAO-{usuarioId:N}",
            Email = $"{usuarioId:N}@test.local",
            NormalizedEmail = $"{usuarioId:N}@TEST.LOCAL",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        });

        dbContext.SaveChanges();
    }
}
