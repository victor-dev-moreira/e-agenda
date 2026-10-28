using eAgenda.Dominio.Compartilhado;
namespace eAgenda.Dominio.Modulos.Tarefas;

public sealed class ItemTarefa : EntidadeBase<ItemTarefa>
{
    public string Titulo { get; set; } = string.Empty;
    public Guid TarefaId { get; set; }
    public bool Concluido { get; set; }
    public Tarefa Tarefa { get; set; } = null!;

    public ItemTarefa()
    {
    }
    public ItemTarefa(Tarefa tarefa, string titulo) : this()
    {
        TarefaId = tarefa.Id;
        Tarefa = tarefa;
        Titulo = titulo;
    }

    public void Concluir()
    {
        Concluido = true;
    }

    public void RemoverConclusao()
    {
        Concluido = false;
    }

    public override void Atualizar(ItemTarefa entidadeAtualizada)
    {
        Titulo = entidadeAtualizada.Titulo;
        Concluido = entidadeAtualizada.Concluido;
    }

    public override List<ErroValidacao> Validar()
    {
        List<ErroValidacao> erros = [];

        if (string.IsNullOrWhiteSpace(Titulo) || Titulo.Length < 2 || Titulo.Length > 100)
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Titulo),
                Mensagem = "O campo \"Titulo\" deve ser conter entre 2 a 100 caracteres!"
            });
        }

        if (Tarefa == null)
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Tarefa),
                Mensagem = "O campo \"Tarefa\" deve conter uma tarefa valida!"
            });
        }
        return erros;
    }
}
