using eAgenda.Aplicacao.Modulo.Tarefas;
using eAgenda.Dominio.Modulos.Tarefas;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Tarefas
{
    public class TarefaController(ServicoTarefa servicoTarefa) : Controller
    {
        [HttpGet]
        public ActionResult Listar()
        {
            List<TarefaDto> dtos = servicoTarefa.SelecionarTodas();

            return View(dtos);
        }

        [HttpGet]

        public ActionResult Cadastrar()
        {
            CadastrarTarefaViewModel viewModel = new(
                string.Empty,
                PrioridadeTarefa.Normal
            );

            return View(viewModel);
        }

        [HttpPost]

        public ActionResult Cadastrar(CadastrarTarefaViewModel viewModel)
        {
            CadastrarTarefaDto dto = new(
           viewModel.Titulo,
           viewModel.Prioridade
       );

            Result<Guid> resultado = servicoTarefa.Cadastrar(dto);

            if (resultado.IsFailed)
            {
                foreach (IError erro in resultado.Errors)
                {
                    string campo = erro.Metadata["Campo"].ToString() ?? string.Empty;

                    ModelState.AddModelError(string.Empty, erro.Message);
                }

                return View(viewModel);
            }

            return RedirectToAction(nameof(Listar));
        }

        [HttpGet]

        public ActionResult Editar()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Editar(EditarTarefaViewModel viewModel)
        {
            return RedirectToAction(nameof(Listar));
        }

        [HttpGet]

        public ActionResult Excluir()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Excluir(ExcluirTarefaViewModel viewModel)
        {
            return RedirectToAction(nameof(Listar));
        }

        public TarefaDto SelecionarPorId(Guid id)
        {
            return null;
        }

        public List<TarefaDto> SelecionarTodas()
        {
            return [];
        }

    }
}
