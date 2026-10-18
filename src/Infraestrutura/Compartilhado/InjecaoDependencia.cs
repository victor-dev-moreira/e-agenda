using eAgenda.Dominio.Modulos.Categorias;
using eAgenda.Dominio.Modulos.Despesas;
using eAgenda.Infraestrutura.Compartilhado.Orm;
using eAgenda.Infraestrutura.Modulos.Categorias;
using eAgenda.Infraestrutura.Modulos.Despesas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eAgenda.Infraestrutura.Compartilhado;

public static class InjecaoDependencia
{
    public static void AdicionadoCamadaInfraestrutura(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EAgendaDbContext>(options =>
        {
            string? connectionString = configuration.GetConnectionString("SqlServer");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("A connectionStrings:SqlServer não foi configurada.");

            options.UseSqlServer(connectionString, cfg => cfg.EnableRetryOnFailure(3));
        });

        services.AddScoped<IRepositorioCategoria, RepositorioCategoriaEmOrm>();
        services.AddScoped<IRepositorioDespesa, RepositorioDespesaEmOrm>();
    }
}
