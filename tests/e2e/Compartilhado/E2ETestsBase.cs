using Microsoft.Playwright;

namespace eAgenda.Testes.e2e.Compartilhado;

public abstract class E2ETestsBase
{
    private TestApplicationFactory applicationFactory = null!;
    private IBrowser browser = null!;
    private IPlaywright playwright = null!;
    protected IPage page = null!;
    private IBrowserContext browserContext = null!;
    protected string? baseUrl = null;

    [TestInitialize]
    public async Task InicializarTeste()
    {
        applicationFactory = new TestApplicationFactory();
        await applicationFactory.StartAsync();

        baseUrl = applicationFactory.BaseUrl;

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

        await applicationFactory.DisposeAsync();
    }

    protected async Task CadastrarUsuarioAsync()
    {
        await page.GotoAsync($"{baseUrl}/Autenticacao/Cadastro");

        await page.GetByLabel("E-mail").FillAsync("victor@gmail.com");
        await page.GetByLabel("Senha", new() { Exact = true }).FillAsync("300300@Hugo");
        await page.GetByLabel("Confirmar Senha").FillAsync("300300@Hugo");
        await page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();

        await page.GotoAsync($"{baseUrl}/Autenticacao/Login");

        await page.GetByLabel("E-mail").FillAsync("victor@gmail.com");
        await page.GetByLabel("Senha").FillAsync("300300@Hugo");
        await page.GetByRole(AriaRole.Button, new() { Name = "Entrar" }).ClickAsync();
    }
}
