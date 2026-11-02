using eAgenda.WebApp.Aplicacao.Compartilhado;

namespace eAgenda.Dominio.Modulos.Contatos;

public interface IRepositorioContato : IRepositorio<Contato>
{
    bool ExisteCategoriaPorTitulo(string tituloCategoria, Guid? idIgnorado = null);
}
