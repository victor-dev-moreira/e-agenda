using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Infraestrutura.Compartilhado.Orm;

namespace eAgenda.Infraestrutura.Modulos.Contatos;

public sealed class RepositorioContatoEmOrm(EAgendaDbContext dbContext) :
    RepositorioBaseEmOrm<Contato>(dbContext), IRepositorioContato
{
    public bool ExisteCategoriaPorTitulo(string tituloCategoria, Guid? idIgnorado = null)
    {
        throw new NotImplementedException();
    }

    public override List<Contato> SelecionarTodos()
    {
        return registros.OrderBy(c => c.Nome).ToList();
    }
}
