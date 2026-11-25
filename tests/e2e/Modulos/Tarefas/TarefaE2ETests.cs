using eAgenda.Testes.e2e.Compartilhado;
using Microsoft.Playwright;
using Microsoft.Testing.Platform.Builder;

namespace eAgenda.Testes.e2e;

[TestClass]
public sealed class TarefaE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task Deve_Cadastrar_Tarefa_Corretamente()
    {
        // Arranjo

        await CadastrarUsuarioAsync();

        await page.GotoAsync($"{baseUrl}/Tarefa/Cadastrar");

        // Ação

        await page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await page.GetByLabel("Prioridade").SelectOptionAsync("Alta");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator card = page.Locator(".card").Filter(new() { HasText = "Lavar o cachorro" });

        // Asserção
        await Assertions.Expect(card).ToBeVisibleAsync();


    }

    public async Task Deve_Editar_Tarefa_Corretamente()
    {
        // Arranjo

        await CadastrarUsuarioAsync();

        await page.GotoAsync($"{baseUrl}/Tarefa/Cadastrar");

        await page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await page.GetByLabel("Prioridade").SelectOptionAsync("Alta");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator card = page.Locator(".card").Filter(new() { HasText = "Lavar o cachorro" });


        await card.GetByRole(AriaRole.Link, new() { Name = "Editar", Exact = true }).ClickAsync();

        // Ação
        await page.GetByLabel("Título").FillAsync("O Cachorro");
        await page.GetByLabel("Prioridade").SelectOptionAsync("Baixa");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator cardEditado = page.Locator(".card").Filter(new() { HasText = "O Cachorro" });

        // Asserção



        await Assertions.Expect(cardEditado).ToHaveCountAsync(1);
        await Assertions.Expect(cardEditado.Locator(".card-title")).ToHaveTextAsync("O Cachorro");
        await Assertions.Expect(cardEditado.GetByText("Baixa", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(card).ToHaveCountAsync(0);
    }

    public async Task Deve_Excluir_Tarefa_Corretamente()
    {
        // Arranjo

        await CadastrarUsuarioAsync();

        await page.GotoAsync($"{baseUrl}/Tarefa/Cadastrar");

        await page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await page.GetByLabel("Prioridade").SelectOptionAsync("Alta");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator card = page.Locator(".card").Filter(new() { HasText = "Lavar o cachorro" });


        await card.GetByRole(AriaRole.Link, new() { Name = "Excluir", Exact = true }).ClickAsync();

        // Ação

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        // Asserção

        await Assertions.Expect(card).ToHaveCountAsync(0);
        await Assertions.Expect(page.GetByRole(AriaRole.Alert)).ToHaveTextAsync("Nenhuma tarefa cadastrada");

    }
}
