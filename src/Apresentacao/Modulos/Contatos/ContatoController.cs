using eAgenda.Aplicacao.Modulos.Contatos;
using eAgenda.Dominio.Modulos.Contatos;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eAgenda.WebApp.Modulos.Contatos;

public class ContatoController(ServicoContato servicoContato) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ContatoDto> dtos = servicoContato.SelecionarTodos();

        return View(dtos);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarContatoViewModel viewModel = new CadastrarContatoViewModel(
            string.Empty,
            string.Empty,
            string.Empty,
            null,
            null
        );

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarContatoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        CadastrarContatoDto dto = new(viewModel.Nome, viewModel.Email, viewModel.Telefone, viewModel.Cargo, viewModel.Empresa);

        Result<Guid> resultado = servicoContato.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            foreach (IError erro in resultado.Errors)
            {
                string campo = erro.Metadata["Campo"].ToString() ?? string.Empty;

                ModelState.AddModelError(campo, erro.Message);
            }

            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]

    public ActionResult Editar(Guid id)
    {
        Result<ContatoDto> resultado = servicoContato.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        ContatoDto dto = resultado.Value;

        EditarContatoViewModel viewModel = new(
            dto.Id,
            dto.Nome,
            dto.Email,
            dto.Telefone,
            dto.Cargo,
            dto.Empresa
        );

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Editar(EditarContatoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        EditarContatoDto dto = new(
            viewModel.Id,
            viewModel.Nome ?? string.Empty,
            viewModel.Email ?? string.Empty,
            viewModel.Telefone ?? string.Empty,
            viewModel.Cargo,
            viewModel.Empresa
        );

        Result resultado = servicoContato.Editar(dto);

        if (resultado.IsFailed)
        {
            string mensagemErro = resultado.Errors.Select(e => e.Message).First();

            foreach (var erro in resultado.Errors)
            {
                string campo = erro.Metadata["Campo"].ToString() ?? string.Empty;

                ModelState.AddModelError(campo, erro.Message);
            }

            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        Result<ContatoDto> resultado = servicoContato.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        ContatoDto dto = resultado.Value;

        ExcluirContatoViewModel viewModel = new(dto.Id, dto.Nome, dto.Email, dto.Telefone, dto.Cargo, dto.Empresa);

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirContatoViewModel viewModel)
    {
        Result resultado = servicoContato.Excluir(viewModel.Id);

        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors.First().Message;

        return RedirectToAction(nameof(Listar));
    }
}
