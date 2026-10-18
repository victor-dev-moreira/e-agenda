using System;
using eAgenda.Dominio.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.Compartilhado.Orm;

public abstract class RepositorioBaseEmOrm<T>(EAgendaDbContext dbContext) where T : EntidadeBase<T>
{
    protected readonly DbSet<T> registros = dbContext.Set<T>();

    public void Cadastrar(T entidade)
    {
        registros.Add(entidade);

        dbContext.SaveChanges();
    }

    public bool Editar(Guid idSelecionado, T entidadeAtualizada)
    {
        T? entidadeSelecionada = SelecionarPorId(idSelecionado);

        if (entidadeSelecionada == null)
            return false;

        entidadeSelecionada.Atualizar(entidadeAtualizada);

        return true;
    }

    public bool Excluir(Guid idSelecionado)
    {
        T? entidadeSelecionada = SelecionarPorId(idSelecionado);

        if (entidadeSelecionada == null)
            return false;

        registros.Remove(entidadeSelecionada);

        dbContext.SaveChanges();
        return true;
    }
    public virtual T? SelecionarPorId(Guid idSelecionado)
    {
        return registros.SingleOrDefault(c => c.Id == idSelecionado);

    }

    public virtual List<T> SelecionarTodos()
    {
        return registros
            .ToList();
    }

}
