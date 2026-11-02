using eAgenda.Aplicacao.Modulos.Compromissos;
using eAgenda.Aplicacao.Modulos.Contatos;
using eAgenda.Dominio.Modulos.Compromissos;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Compromissos;

public class CompromissoController(
    ServicoCompromisso servicoCompromisso,
    ServicoContato servicoContato
) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<CompromissoDto> dtos = servicoCompromisso.SelecionarTodos();

        return View(dtos);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarCompromissoViewModel cadastrarVm = new CadastrarCompromissoViewModel(
            string.Empty,
            DateOnly.FromDateTime(DateTime.Today),
            TimeSpan.Zero,
            TimeSpan.Zero,
            TipoCompromisso.Presencial,
            null,
            null,
            null
        );

        return View(cadastrarVm with { Contatos = SelecionarContatos() });
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCompromissoViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View(cadastrarVm);

        CadastrarCompromissoDto dto = new(
            cadastrarVm.Assunto,
            cadastrarVm.DataOcorrencia,
            cadastrarVm.HoraInicio,
            cadastrarVm.HoraTermino,
            cadastrarVm.Tipo,
            cadastrarVm.Local,
            cadastrarVm.Link,
            cadastrarVm.ContatoId
        );

        Result<Guid> resultado = servicoCompromisso.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            foreach (IError erro in resultado.Errors)
            {
                string campo = erro.Metadata["Campo"].ToString() ?? string.Empty;

                ModelState.AddModelError(campo, erro.Message);
            }

            return View(cadastrarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<CompromissoDto> resultado = servicoCompromisso.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        CompromissoDto dto = resultado.Value;

        EditarCompromissoViewModel viewModel = new(
            dto.Id,
            dto.Assunto,
            dto.DataOcorrencia,
            dto.HoraInicio,
            dto.HoraTermino,
            dto.Tipo,
            dto.Local,
            dto.Link,
            dto.ContatoId
        );

        return View(viewModel with { Contatos = SelecionarContatos() });
    }

    [HttpPost]
    public ActionResult Editar(EditarCompromissoViewModel viewModel)
    {
        EditarCompromissoDto dto = new(
            viewModel.Id,
            viewModel.Assunto ?? string.Empty,
            viewModel.DataOcorrencia,
            viewModel.HoraInicio,
            viewModel.HoraTermino,
            viewModel.Tipo,
            viewModel.Local,
            viewModel.Link,
            viewModel.ContatoId
        );

        Result resultado = servicoCompromisso.Editar(dto);

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
    public ActionResult Excluir(Guid id)
    {
        Result<CompromissoDto> resultado = servicoCompromisso.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        CompromissoDto dto = resultado.Value;

        ExcluirCompromissoViewModel viewModel = new(
            dto.Id,
            dto.Assunto ?? string.Empty,
            dto.DataOcorrencia,
            dto.HoraInicio,
            dto.HoraTermino,
            dto.Tipo,
            dto.Local,
            dto.Link,
            dto.ContatoId,
            dto.ContatoNome
        );

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCompromissoViewModel viewModel)
    {
        Result resultado = servicoCompromisso.Excluir(viewModel.Id);

        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors.First().Message;

        return RedirectToAction(nameof(Listar));
    }

    private List<OpcaoContatoViewModel> SelecionarContatos()
    {
        List<OpcaoContatoDto> dtos = servicoContato
            .SelecionarTodos()
            .Select(c => new OpcaoContatoDto(c.Id, c.Nome))
            .ToList();

        return dtos.Select(c => new OpcaoContatoViewModel(c.Id, c.Nome)).ToList();
    }
}
