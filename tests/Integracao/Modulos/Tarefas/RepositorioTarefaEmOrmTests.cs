using eAgenda.Dominio.Modulos.Tarefas;
using eAgenda.Infraestrutura.Modulos.Tarefas;

namespace eAgenda.Testes.Integracao.Modulos.Tarefas;

[TestClass]
public sealed partial class RepositorioTarefaEmOrmTests : RepositorioEmOrmTestsBase
{

    [TestMethod]
    public void Cadastrar_DevePersistir_TarefaEItensNoBanco()
    {
        // Arranjo
        Guid tarefaId = Guid.CreateVersion7();

        Tarefa tarefa = new("Preparar", PrioridadeTarefa.Alta)
        {
            Id = tarefaId,
            UsuarioId = usuarioId,
            DataCriacao = new DateOnly(2026, 11, 09)
        };

        tarefa.AdicionarItem(new ItemTarefa(tarefa, "Revisar") { UsuarioId = usuarioId });
        tarefa.AdicionarItem(new ItemTarefa(tarefa, "Montagem") { UsuarioId = usuarioId, Concluido = true });

        RepositorioTarefaEmOrm repositorioTarefa = new(dbContext);

        // Ação
        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefaId);

        // Asserção
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("Preparar", tarefaSelecionada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, tarefaSelecionada.Prioridade);
        Assert.AreEqual(new DateOnly(2026, 11, 09), tarefaSelecionada.DataCriacao);
        Assert.AreEqual(50, tarefaSelecionada.PercentualConcluido);
        Assert.HasCount(2, tarefaSelecionada.Itens);

        CollectionAssert.AreEqual(
            new[] { "Montagem", "Revisar" },
            tarefaSelecionada.Itens.Select(i => i.Titulo).ToArray()

        );
    }

    [TestMethod]

    public void Editar_DeveAtualizar_TarefaEItensNoBanco()
    {
        // Arranjo
        Guid tarefaId = Guid.CreateVersion7();

        Tarefa tarefaOriginal = new("Preparar", PrioridadeTarefa.Alta)
        {
            Id = tarefaId,
            UsuarioId = usuarioId,
            DataCriacao = new DateOnly(2026, 11, 09)
        };

        tarefaOriginal.AdicionarItem(new ItemTarefa(tarefaOriginal, "Revisar") { UsuarioId = usuarioId });

        RepositorioTarefaEmOrm repositorioTarefa = new(dbContext);

        repositorioTarefa.Cadastrar(tarefaOriginal);
        dbContext.ChangeTracker.Clear();

        Tarefa tarefaEditada = new("Preparar Teste", PrioridadeTarefa.Normal)
        {
            Id = tarefaId,
            UsuarioId = usuarioId,
            DataCriacao = new DateOnly(2026, 11, 11)
        };

        tarefaEditada.AdicionarItem(new ItemTarefa(tarefaEditada, "Revisar Novo") { UsuarioId = usuarioId, Concluido = true });

        // Ação
        bool conseguiuEditar = repositorioTarefa.Editar(tarefaId, tarefaEditada);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefaId);

        // Asserção

        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("Preparar Teste", tarefaSelecionada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Normal, tarefaSelecionada.Prioridade);
        Assert.AreEqual(new DateOnly(2026, 11, 11), tarefaSelecionada.DataCriacao);
        Assert.HasCount(1, tarefaSelecionada.Itens);
        Assert.IsTrue(tarefaSelecionada.Concluido);
        Assert.IsNotNull(tarefaSelecionada.DataConclusao);
    }

    [TestMethod]
    public void Excluir_DeveApagar_TarefaEItensNoBanco()
    {
        // Arranjo
        Guid tarefaId = Guid.CreateVersion7();

        Tarefa tarefaOriginal = new("Preparar", PrioridadeTarefa.Alta)
        {
            Id = tarefaId,
            UsuarioId = usuarioId,
            DataCriacao = new DateOnly(2026, 11, 09)
        };

        tarefaOriginal.AdicionarItem(new ItemTarefa(tarefaOriginal, "Revisar") { UsuarioId = usuarioId });

        RepositorioTarefaEmOrm repositorioTarefa = new(dbContext);

        repositorioTarefa.Cadastrar(tarefaOriginal);
        dbContext.ChangeTracker.Clear();

        // Ação
        bool conseguiuExcluir = repositorioTarefa.Excluir(tarefaId);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefaId);

        int quantidadeItensTarefa = dbContext.ItemTarefa.Count(i => i.TarefaId == tarefaId);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(tarefaSelecionada);
        Assert.AreEqual(0, quantidadeItensTarefa);


    }

}
