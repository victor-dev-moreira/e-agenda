using eAgenda.Dominio.Compartilhado;
namespace eAgenda.Dominio.Modulos.Tarefas;

public class Tarefa : EntidadeBase<Tarefa>
{
    public string Titulo { get; set; } = string.Empty;
    public PrioridadeTarefa Prioridade { get; set; }
    public DateOnly DataCriacao { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly? DataConclusao { get; set; }
    public int PercentualConcluido { get; set; }
    public bool Concluido { get; set; }
    public List<ItemTarefa> Itens { get; set; } = [];

    public Tarefa() { }

    public Tarefa(string titulo, PrioridadeTarefa prioridade)
    {
        Titulo = titulo;
        Prioridade = prioridade;

        RecalcularConclusao();
    }
    public void AdicionarItem(ItemTarefa item)
    {
        Itens.Add(item);

        RecalcularConclusao();
    }

    public bool RemoverItem(Guid itemId)
    {
        ItemTarefa? item = Itens.FirstOrDefault(i => i.Id == itemId);

        if (item == null)
            return false;

        Itens.Remove(item);

        RecalcularConclusao();

        return true;
    }

    public bool AlterarConclusaoItem(Guid itemId, bool concluido)
    {
        ItemTarefa? item = Itens.FirstOrDefault(i => i.Id == itemId);

        if (item == null)
            return false;

        if (concluido)
            item.Concluir();
        else
            item.RemoverConclusao();

        RecalcularConclusao();

        return true;

    }

    public void RecalcularConclusao()
    {
        if (Itens.Count == 0)
        {
            PercentualConcluido = 0;
            Concluido = false;
            DataConclusao = null;

            return;
        }

        int itensConcluidos = Itens.Count(i => i.Concluido);

        PercentualConcluido = (int)Math.Round((decimal)itensConcluidos / Itens.Count * 100);

        Concluido = PercentualConcluido == 100;

        DataConclusao = Concluido ? DataConclusao ?? DateOnly.FromDateTime(DateTime.Today) : null;
    }
    public override void Atualizar(Tarefa entidadeAtualizada)
    {
        Titulo = entidadeAtualizada.Titulo;
        Prioridade = entidadeAtualizada.Prioridade;
        DataCriacao = entidadeAtualizada.DataCriacao;
        DataConclusao = entidadeAtualizada.DataConclusao;
        Concluido = entidadeAtualizada.Concluido;
        PercentualConcluido = entidadeAtualizada.PercentualConcluido;
        Itens = entidadeAtualizada.Itens;
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

        if (!Enum.IsDefined(Prioridade))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Prioridade),
                Mensagem = "O campo \"Prioridade\" deve ser preenchido!"
            });
        }

        return erros;
    }
}
