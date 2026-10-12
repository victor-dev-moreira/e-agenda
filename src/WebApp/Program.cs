using eAgenda.Aplicacao.Compartilhado;
using eAgenda.Infraestrutura.Compartilhado;
using eAgenda.Infraestrutura.Compartilhado.Orm;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Configura Camda de Aplicacão

builder.Services.AdicionarCamadaAplicacao();
builder.Services.AdicionadoCamadaInfraestrutura(builder.Configuration);

// Config camada de Apresentacão
builder.Services.AddControllersWithViews().AddRazorOptions(options =>
{
    // Reseta a configuração padrão do MVC
    options.ViewLocationFormats.Clear();

    // Localização das Views dos módulos: Modulos/ModuloAluno/Apresentacao/Views/Listar.cshtml
    options.ViewLocationFormats.Add("/Modulos/{1}s/Views/{0}.cshtml");

    // Localização das Views compartilhadas: /Compartilhado/Apresentacao/Views/_Layout.cshtml
    options.ViewLocationFormats.Add("/Compartilhado/Views/{0}.cshtml");
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<EAgendaDbContext>();

    if (dbContext.Database.IsSqlServer())
        dbContext.Database.Migrate();
}


app.UseRouting();
app.MapDefaultControllerRoute();

app.Run();
