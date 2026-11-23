using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.Modulos.Categorias;
using eAgenda.Dominio.Modulos.Compromissos;
using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Dominio.Modulos.Despesas;
using eAgenda.Dominio.Modulos.Tarefas;
using eAgenda.Infraestrutura.Compartilhado.Identity;
using eAgenda.Infraestrutura.Modulos.Categorias;
using eAgenda.Infraestrutura.Modulos.Compromissos;
using eAgenda.Infraestrutura.Modulos.Contatos;
using eAgenda.Infraestrutura.Modulos.Despesas;
using eAgenda.Infraestrutura.Modulos.Tarefas;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.Compartilhado.Orm;

public sealed class EAgendaDbContext(
    DbContextOptions<EAgendaDbContext> options,
    IProvedorUsuarioAtual usuarioAtual
) : IdentityDbContext<UsuarioIdentity, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Despesa> Despesas => Set<Despesa>();
    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
    public DbSet<ItemTarefa> ItemTarefa => Set<ItemTarefa>();
    public DbSet<Contato> Contatos => Set<Contato>();
    public DbSet<Compromisso> Compromissos => Set<Compromisso>();

    public override int SaveChanges()
    {
        Guid usuarioId = usuarioAtual.Id;

        foreach (var entrada in ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Added &&
                                 e.Metadata.FindProperty(nameof(EntidadeBase<object>.UsuarioId)) is not null))
        {
            var propriedadeUsuarioId = entrada.Property(nameof(EntidadeBase<object>.UsuarioId));

            if ((Guid)propriedadeUsuarioId.CurrentValue! == Guid.Empty)
                propriedadeUsuarioId.CurrentValue = usuarioId;
        }

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
        modelBuilder.ApplyConfiguration(new DespesaConfiguration());
        modelBuilder.ApplyConfiguration(new TarefaConfiguration());
        modelBuilder.ApplyConfiguration(new ItemTarefaConfiguration());
        modelBuilder.ApplyConfiguration(new ContatoConfiguration());
        modelBuilder.ApplyConfiguration(new CompromissoConfiguration());

        modelBuilder.Entity<Categoria>().HasQueryFilter(c => c.UsuarioId == usuarioAtual.Id);
    }

}
