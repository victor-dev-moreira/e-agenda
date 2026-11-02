using eAgenda.Dominio.Modulos.Compromissos;
using eAgenda.Infraestrutura.Compartilhado.Orm;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.Modulos.Compromissos;

public sealed class RepositorioCompromissoEmOrm(EAgendaDbContext dbContext) :
    RepositorioBaseEmOrm<Compromisso>(dbContext), IRepositorioCompromisso
{
    public override Compromisso? SelecionarPorId(Guid idSelecionado)
    {
        return registros
            .Include(c => c.Contato)
            .SingleOrDefault(c => c.Id == idSelecionado);
    }

    public override List<Compromisso> SelecionarTodos()
    {
        return registros
            .Include(c => c.Contato)
            .OrderBy(c => c.DataOcorrencia)
            .ThenBy(c => c.HoraInicio)
            .ToList();
    }
}
