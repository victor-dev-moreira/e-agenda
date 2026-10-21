using eAgenda.Aplicacao.Modulo.Categorias;
using eAgenda.Aplicacao.Modulo.Despesas;
using eAgenda.Aplicacao.Modulo.Tarefas;
using Microsoft.Extensions.DependencyInjection;
namespace eAgenda.Aplicacao.Compartilhado;

public static class InjecaoDependencia
{
    public static void AdicionarCamadaAplicacao(this IServiceCollection services)
    {
        services.AddScoped<ServicoCategoria>();
        services.AddScoped<ServicoDespesa>();
        services.AddScoped<ServicoTarefa>();
    }
}

