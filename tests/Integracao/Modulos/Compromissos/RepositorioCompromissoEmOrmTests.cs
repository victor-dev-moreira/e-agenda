using eAgenda.Dominio.Modulos.Compromissos;
using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Infraestrutura.Modulos.Compromissos;
using eAgenda.Testes.Integracao.Modulos.Tarefas;

namespace eAgenda.Testes.Integracao.Modulos.Compromissos;

[TestClass]
public sealed class RepositorioCompromissoEmOrmTests : RepositorioEmOrmTestsBase
{
    [TestMethod]
    public void Cadastrar_DevePersistir_CompromissoNoBanco()
    {
        // Arranjo

        Guid compromissoId = Guid.CreateVersion7();

        Compromisso compromisso = new(
            "Reunião",
            new DateOnly(2026, 11, 10),
            new TimeSpan(11, 00, 00),
            new TimeSpan(12, 00, 00),
            TipoCompromisso.Remoto,
            null,
            "https://meet.google.com",
            null
        )
        {
            Id = compromissoId,
            UsuarioId = usuarioId
        };

        RepositorioCompromissoEmOrm repositorioCompromisso = new(dbContext);

        // Ação

        repositorioCompromisso.Cadastrar(compromisso);
        dbContext.ChangeTracker.Clear();

        Compromisso? compromissoSelecionado = repositorioCompromisso.SelecionarPorId(compromissoId);

        // Asserção

        Assert.IsNotNull(compromissoSelecionado);
        Assert.AreEqual(new DateOnly(2026, 11, 10), compromissoSelecionado.DataOcorrencia);
        Assert.AreEqual(new TimeSpan(11, 00, 00), compromissoSelecionado.HoraInicio);
        Assert.AreEqual(new TimeSpan(12, 00, 00), compromissoSelecionado.HoraTermino);
        Assert.AreEqual(TipoCompromisso.Remoto, compromissoSelecionado.Tipo);
        Assert.IsNull(compromissoSelecionado.Local);
        Assert.IsNotNull(compromissoSelecionado.Link);
        Assert.AreEqual("https://meet.google.com", compromissoSelecionado.Link);


    }

    [TestMethod]
    public void Cadastrar_DevePersistir_Compromisso_ComContatoNoBanco()
    {
        // Arranjo
        Guid contatoId = Guid.CreateVersion7();

        Contato contato = new(
            "Vasco",
            "vasco@gmail.com",
            "(41) 99999-8888",
            "Teste",
            "Futebol"
        )
        {
            Id = contatoId,
            UsuarioId = usuarioId
        };

        Guid compromissoId = Guid.CreateVersion7();

        Compromisso compromisso = new(
            "Reunião",
            new DateOnly(2026, 11, 10),
            new TimeSpan(11, 00, 00),
            new TimeSpan(12, 00, 00),
            TipoCompromisso.Presencial,
            "São Vicente",
            null,
            contato
        )
        {
            Id = compromissoId,
            UsuarioId = usuarioId
        };

        RepositorioCompromissoEmOrm repositorioCompromisso = new(dbContext);

        // Ação

        repositorioCompromisso.Cadastrar(compromisso);
        dbContext.ChangeTracker.Clear();

        Compromisso? compromissoSelecionado = repositorioCompromisso.SelecionarPorId(compromissoId);

        // Asserção

        Assert.IsNotNull(compromissoSelecionado);
        Assert.AreEqual(new DateOnly(2026, 11, 10), compromissoSelecionado.DataOcorrencia);
        Assert.AreEqual(new TimeSpan(11, 00, 00), compromissoSelecionado.HoraInicio);
        Assert.AreEqual(new TimeSpan(12, 00, 00), compromissoSelecionado.HoraTermino);
        Assert.AreEqual(TipoCompromisso.Presencial, compromissoSelecionado.Tipo);
        Assert.IsNull(compromissoSelecionado.Link);
        Assert.AreEqual("São Vicente", compromissoSelecionado.Local);
        Assert.IsNotNull(compromissoSelecionado.Local);
        Assert.IsNotNull(compromissoSelecionado.Contato);
        Assert.AreEqual(contato, compromissoSelecionado.Contato);

    }
}
