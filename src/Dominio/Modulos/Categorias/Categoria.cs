using eAgenda.Dominio.Compartilhado;

namespace eAgenda.Dominio.Modulos.Categorias;

public sealed class Categoria : EntidadeBase<Categoria>
{

    public string Titulo { get; set; } = string.Empty;

    public Categoria() { }

    public Categoria(string titulo) : this()
    {
        Titulo = titulo;
    }
    public override void Atualizar(Categoria entidadeAtualizada)
    {
        Titulo = entidadeAtualizada.Titulo;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Titulo) || Titulo.Length < 2 || Titulo.Length > 100)
            erros.Add("O campo \"Titulo\" deve conter entre 2 e 100 caracteres!");


        return erros;
    }
}
