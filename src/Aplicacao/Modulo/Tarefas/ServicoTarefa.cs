using System;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.Modulos.Tarefas;
using FluentResults;

namespace eAgenda.Aplicacao.Modulo.Tarefas;

public sealed class ServicoTarefa(IRepositorioTarefa repositorioTarefa)
{

    public Result<Guid> Cadastrar(CadastrarTarefaDto dto)
    {
        Tarefa tarefa = new(dto.Titulo, dto.Prioridade);

        List<ErroValidacao> erros = tarefa.Validar();

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
            .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo)).ToList();

            return Result.Fail(errosResultado);
        }
        repositorioTarefa.Cadastrar(tarefa);

        return Result.Ok(tarefa.Id);
    }

    public Result<Guid> Editar(EditarTarefaDto dto)
    {
        return Result.Ok();
    }

    public Result<Guid> Excluir(Guid id)
    {
        return Result.Ok();
    }

    public List<TarefaDto> SelecionarTodas()
    {
        return repositorioTarefa
        .SelecionarTodos()
        .Select(t => new TarefaDto(t.Id, t.Titulo, t.Prioridade, t.DataCriacao))
        .ToList();
    }
}
