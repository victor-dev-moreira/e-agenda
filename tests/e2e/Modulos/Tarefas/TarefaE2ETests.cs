using Microsoft.Playwright;

namespace eAgenda.Testes.e2e;

[TestClass]
public sealed class TarefaE2ETests
{
    private IBrowser browser = null!;
    private IPlaywright playwright = null!;
    private IPage page = null!;
    private IBrowserContext browserContext = null!;

    [TestInitialize]
    public async Task InicializarTeste()
    {
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        browserContext = await browser.NewContextAsync();
        page = await browserContext.NewPageAsync();
    }

    [TestCleanup]

    public async Task FinalizarTeste()
    {
        await browser.DisposeAsync();
        playwright.Dispose();
    }

    [TestMethod]
    public async Task Deve_Cadastrar_Tarefa_Corretamente()
    {
        // Arranjo
        await page.GotoAsync("http://localhost:5298/Tarefa/Cadastrar");

        await page.GetByLabel("E-mail").FillAsync("victor@gmail.com");
        await page.GetByLabel("Senha").FillAsync("300300@Hugo");
        await page.GetByRole(AriaRole.Button, new() { Name = "Entrar" }).ClickAsync();

        await page.GotoAsync("http://localhost:5298/Tarefa/Cadastrar");

        // Ação

        await page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await page.GetByLabel("Prioridade").SelectOptionAsync("Alta");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator card = page.Locator(".card").Filter(new() { HasText = "Lavar o cachorro" });

        // Asserção

        Assert.IsTrue(await card.IsVisibleAsync());
    }

}
