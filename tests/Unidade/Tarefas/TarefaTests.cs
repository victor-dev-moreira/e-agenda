using eAgenda.Dominio.Modulos.Tarefas;

namespace eAgenda.Testes.Unidade.Tarefas;

[TestClass]
public sealed class TarefaTests
{
    // Padrão AAA (Arranjo, Ação e Asserção)
    [TestMethod]
    public void Construtor_ComDadosValidos_DeveInicializarTarefa_Pendente()
    {
        // Arranjo
        Tarefa tarefa;

        // Ação
        tarefa = new("Estudar", PrioridadeTarefa.Alta);

        // Asserção
        Assert.AreEqual("Estudar", tarefa.Titulo);

    }

    [TestMethod]

    public void AdicionarItem_Deve_RecalcularConclusao_TarefaPendente()
    {
        // Arranjo
        Tarefa tarefa = new("Estudar", PrioridadeTarefa.Alta);
        ItemTarefa item = new(tarefa, "Organizar Material de Trabalho");
        item.Concluido = true;

        // Ação
        tarefa.AdicionarItem(item);

        // Asserção
        Assert.HasCount(1, tarefa.Itens);
        Assert.AreEqual(item, tarefa.Itens[0]);

        Assert.AreEqual(100, tarefa.PercentualConcluido);
        Assert.IsTrue(tarefa.Concluido);
        Assert.AreEqual(DateOnly.FromDateTime(DateTime.Now), tarefa.DataConclusao);
    }

    [TestMethod]

    public void RemoverItem_Deve_RecalcularConclusao_TarefaConcluido()
    {
        // Arranjo
        Tarefa tarefa = new("Estudar", PrioridadeTarefa.Alta);
        ItemTarefa item = new(tarefa, "Organizar Material de Trabalho");
        item.Concluido = true;

        tarefa.AdicionarItem(item);

        // Ação
        tarefa.RemoverItem(item.Id);

        // Asserção
        Assert.HasCount(0, tarefa.Itens);

        Assert.AreEqual(0, tarefa.PercentualConcluido);
        Assert.IsFalse(tarefa.Concluido);
        Assert.IsNull(tarefa.DataConclusao);
    }

    [TestMethod]

    public void AlterarConclusaoItem_Deve_RecalcularConclusao_TarefaPendente()
    {
        // Arranjo
        Tarefa tarefa = new("Estudar", PrioridadeTarefa.Alta);
        ItemTarefa item = new(tarefa, "Organizar Material de Trabalho");

        tarefa.AdicionarItem(item);

        // Ação
        tarefa.AlterarConclusaoItem(item.Id, true);

        // Asserção
        Assert.HasCount(1, tarefa.Itens);

        Assert.AreEqual(100, tarefa.PercentualConcluido);
        Assert.IsTrue(tarefa.Concluido);
        Assert.AreEqual(DateOnly.FromDateTime(DateTime.Now), tarefa.DataConclusao);
    }

}
