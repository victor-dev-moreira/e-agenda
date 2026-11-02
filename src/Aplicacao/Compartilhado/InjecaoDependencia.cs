using eAgenda.Aplicacao.Modulo.Categorias;
using eAgenda.Aplicacao.Modulo.Despesas;
using eAgenda.Aplicacao.Modulo.Tarefas;
using eAgenda.Aplicacao.Modulos.Compromissos;
using eAgenda.Aplicacao.Modulos.Contatos;
using Microsoft.Extensions.DependencyInjection;
namespace eAgenda.Aplicacao.Compartilhado;

public static class InjecaoDependencia
{
    public static void AdicionarCamadaAplicacao(this IServiceCollection services)
    {
        services.AddScoped<ServicoCategoria>();
        services.AddScoped<ServicoDespesa>();
        services.AddScoped<ServicoTarefa>();
        services.AddScoped<ServicoContato>();
        services.AddScoped<ServicoCompromisso>();
    }
}

