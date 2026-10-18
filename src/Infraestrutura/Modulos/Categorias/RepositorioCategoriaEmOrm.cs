using eAgenda.Dominio.Modulos.Categorias;
using eAgenda.Infraestrutura.Compartilhado.Orm;
namespace eAgenda.Infraestrutura.Modulos.Categorias;

public sealed class RepositorioCategoriaEmOrm(EAgendaDbContext dbContext) :
    RepositorioBaseEmOrm<Categoria>(dbContext), IRepositorioCategoria
{
    public override List<Categoria> SelecionarTodos()
    {
        return dbContext.Categorias
        .OrderBy(c => c.Titulo)
        .ToList();
    }

    public bool ExisteCategoriaPorTitulo(string tituloCategoria, Guid? idIgnorado = null)
    {
        return registros.Any(c =>
            c.Id != idIgnorado &&
            !string.IsNullOrWhiteSpace(tituloCategoria) &&
            c.Titulo.ToLower() == tituloCategoria.ToLower()
            );
    }

}
