using eAgenda.Aplicacao.Compartilhado;
using eAgenda.Infraestrutura.Compartilhado;
using eAgenda.Infraestrutura.Compartilhado.Identity;
using eAgenda.Infraestrutura.Compartilhado.Orm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura Camada de Infraestrutura
builder.Services.AdicionarCamadaInfraestrutura(builder.Configuration);

// Configura Camada de Aplicação
builder.Services.AdicionarCamadaAplicacao();

// Configura Camada de Apresentação
builder.Services.AddControllersWithViews().AddRazorOptions(options =>
{
    // Reseta a configuração padrão do MVC
    options.ViewLocationFormats.Clear();

    // Localização das Views dos módulos: Modulos/Alunos/Apresentacao/Views/Listar.cshtml
    options.ViewLocationFormats.Add("/Modulos/{1}s/Views/{0}.cshtml");

    // Localização das Views compartilhadas: /Compartilhado/Apresentacao/Views/_Layout.cshtml
    options.ViewLocationFormats.Add("/Compartilhado/Views/{0}.cshtml");
});

builder.Services.AddIdentity<UsuarioIdentity, IdentityRole<Guid>>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<EAgendaDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Autenticacao/Login";
    options.AccessDeniedPath = "/Autenticacao/Login";
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser().Build();
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
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
