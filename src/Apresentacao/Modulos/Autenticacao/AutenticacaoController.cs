using eAgenda.Infraestrutura.Compartilhado.Identity;
using EscolaDeCursos.WebApp.Modulos.ModuloHome.Apresentacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Autenticacao;

public class AutenticacaoController(
    UserManager<UsuarioIdentity> userManager, // responsável pela criação de usuários e obtenção dos dados do mesmo
    SignInManager<UsuarioIdentity> signInManager // responsável pelo login/logout de usuários já criados
) : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public ActionResult Cadastro()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(HomeController.Index), "Home");

        return View(new CadastroUsuarioViewModel(string.Empty, string.Empty));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> Cadastro(CadastroUsuarioViewModel viewModel)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(HomeController.Index), "Home");

        if (!ModelState.IsValid)
            return View(viewModel);

        UsuarioIdentity usuario = new()
        {
            UserName = viewModel.Email.ToLower().Trim(),
            Email = viewModel.Email.ToLower().Trim(),
        };

        // Operação Assíncrona
        IdentityResult resultado = await userManager.CreateAsync(usuario, viewModel.Senha);

        if (!resultado.Succeeded)
        {
            foreach (IdentityError erro in resultado.Errors)
                ModelState.AddModelError(string.Empty, erro.Description);

            return View(viewModel);
        }

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(HomeController.Index), "Home");

        LoginUsuarioViewModel viewModel = new(
            string.Empty,
            string.Empty,
            false,
            returnUrl
        );

        return View(viewModel);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> Login(LoginUsuarioViewModel viewModel)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(HomeController.Index), "Home");

        if (!ModelState.IsValid)
            return View(viewModel);

        Microsoft.AspNetCore.Identity.SignInResult resultado = await signInManager.PasswordSignInAsync(
            viewModel.Email.Trim(),
            viewModel.Senha,
            viewModel.LembrarMe,
            lockoutOnFailure: true
        );

        if (resultado.Succeeded)
        {
            return Url.IsLocalUrl(viewModel.ReturnUrl)
                ? LocalRedirect(viewModel.ReturnUrl)
                : RedirectToAction(nameof(HomeController.Index), "Home");
        }

        ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");

        return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return RedirectToAction(nameof(Login));
    }
}
