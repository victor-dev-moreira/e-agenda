using eAgenda.Aplicacao.Modulo.Categorias;
using Microsoft.Extensions.DependencyInjection;
namespace eAgenda.Aplicacao.Compartilhado;

public static class InjecaoDependencia
{
    public static void AdicionarCamadaAplicacao(this IServiceCollection services)
    {
        services.AddScoped<ServicoCategoria>();
    }
}

