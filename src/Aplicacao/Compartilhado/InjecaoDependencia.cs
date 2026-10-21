using eAgenda.Aplicacao..Categorias;
using eAgenda.Aplicacao..Despesas;
using eAgenda.Aplicacao..Tarefas;
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

