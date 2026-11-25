using eAgenda.Infraestrutura.Compartilhado.Orm;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eAgenda.Testes.e2e.Compartilhado;

public sealed class TestApplicationFactory : WebApplicationFactory<Program>
{
    public string? BaseUrl { get; private set; }
    public async Task StartAsync()
    {
        StartupTestServer();

        await CleanupTestDatabaseAsync();
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }

    private void StartupTestServer()
    {
        // Inicializam o servidor kestrel em uma porta aleatória da network
        // ex: http://localhost:6510
        UseKestrel(0);
        StartServer();

        // IServer traz informações sobre o servidor em tempo de execução (runtime)
        IServer server = Services.GetRequiredService<IServer>();

        IServerAddressesFeature? serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

        if (serverAddressesFeature is null)
            throw new InvalidOperationException("Não foi possível obter a URL do servidor.");

        BaseUrl = serverAddressesFeature.Addresses.Single();
    }

    private async Task CleanupTestDatabaseAsync()
    {
        using IServiceScope scope = Services.CreateScope();

        EAgendaDbContext dbContext = scope.ServiceProvider.GetRequiredService<EAgendaDbContext>();

        // Apagamos todos os dados das tabelas do banco de testes
        dbContext.Despesas.RemoveRange(dbContext.Despesas.IgnoreQueryFilters());
        dbContext.Categorias.RemoveRange(dbContext.Categorias.IgnoreQueryFilters());
        dbContext.ItemTarefa.RemoveRange(dbContext.ItemTarefa.IgnoreQueryFilters());
        dbContext.Tarefas.RemoveRange(dbContext.Tarefas.IgnoreQueryFilters());
        dbContext.Compromissos.RemoveRange(dbContext.Compromissos.IgnoreQueryFilters());
        dbContext.Contatos.RemoveRange(dbContext.Contatos.IgnoreQueryFilters());

        dbContext.Users.RemoveRange(dbContext.Users.IgnoreQueryFilters());

        await dbContext.SaveChangesAsync();
    }
}
