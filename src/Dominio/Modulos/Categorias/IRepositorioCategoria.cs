using eAgenda.WebApp.Aplicacao.Compartilhado;

namespace eAgenda.Dominio.Modulos.Categorias;

public interface IRepositorioCategoria : IRepositorio<Categoria>
{
    bool ExisteCategoriaPorTitulo(string titulo);
    bool ExistePorNome(string tituloCategoria, Guid? idIgnorado = null);
}
