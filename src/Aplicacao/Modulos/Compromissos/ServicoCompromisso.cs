using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.Modulos.Compromissos;
using eAgenda.Dominio.Modulos.Contatos;
using FluentResults;

namespace eAgenda.Aplicacao.Modulos.Compromissos;

public sealed class ServicoCompromisso(
    IRepositorioCompromisso repositorioCompromisso,
    IRepositorioContato repositorioContato
)
{
    public Result<Guid> Cadastrar(CadastrarCompromissoDto dto)
    {
        Contato? contatoSelecionado = null;

        if (dto.ContatoId.HasValue)
            contatoSelecionado = repositorioContato.SelecionarPorId(dto.ContatoId.Value);

        Compromisso compromisso = new Compromisso(
            dto.Assunto,
            dto.DataOcorrencia,
            dto.HoraInicio,
            dto.HoraTermino,
            dto.Tipo,
            dto.Local,
            dto.Link,
            contatoSelecionado
        );

        List<ErroValidacao> erros = compromisso.Validar();

        bool existeConflitoDeHorario = repositorioCompromisso
            .SelecionarTodos()
            .Any(c =>
                c.DataOcorrencia == dto.DataOcorrencia &&
                dto.HoraInicio < c.HoraTermino &&
                dto.HoraTermino > c.HoraInicio
            );

        if (existeConflitoDeHorario)
            return Result.Fail(new Error(
                "Já existe um compromisso cadastrado neste intervalo de horário."
            ).WithMetadata("Campo", dto.DataOcorrencia));

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo)).ToList();

            return Result.Fail(errosResultado);
        }

        repositorioCompromisso.Cadastrar(compromisso);

        return Result.Ok(compromisso.Id);
    }

    public Result Editar(EditarCompromissoDto dto)
    {
        Contato? contatoSelecionado = null;

        if (dto.ContatoId.HasValue)
            contatoSelecionado = repositorioContato.SelecionarPorId(dto.ContatoId.Value);

        Compromisso compromissoAtualizado = new Compromisso(
            dto.Assunto,
            dto.DataOcorrencia,
            dto.HoraInicio,
            dto.HoraTermino,
            dto.Tipo,
            dto.Local,
            dto.Link,
            contatoSelecionado
        );

        List<ErroValidacao> erros = compromissoAtualizado.Validar();

        bool existeConflitoDeHorario = repositorioCompromisso
            .SelecionarTodos()
            .Any(c =>
                c.Id != dto.Id &&
                c.DataOcorrencia == dto.DataOcorrencia &&
                dto.HoraInicio < c.HoraTermino &&
                dto.HoraTermino > c.HoraInicio
            );

        if (existeConflitoDeHorario)
            return Result.Fail(new Error(
                "Já existe um compromisso cadastrado neste intervalo de horário."
            ).WithMetadata("Campo", dto.DataOcorrencia));

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo)).ToList();

            return Result.Fail(errosResultado);
        }

        bool conseguiuEditar = repositorioCompromisso.Editar(dto.Id, compromissoAtualizado);

        if (!conseguiuEditar)
            return Result.Fail("Não foi possível editar o compromisso.");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        bool conseguiuExcluir = repositorioCompromisso.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Não foi possível excluir o compromisso.");

        return Result.Ok();
    }

    public Result<CompromissoDto> SelecionarPorId(Guid id)
    {
        Compromisso? compromisso = repositorioCompromisso.SelecionarPorId(id);

        if (compromisso == null)
            return Result.Fail("Compromisso não encontrado.");

        return Result.Ok(new CompromissoDto(
            compromisso.Id,
            compromisso.Assunto,
            compromisso.DataOcorrencia,
            compromisso.HoraInicio,
            compromisso.HoraTermino,
            compromisso.Tipo,
            compromisso.Local,
            compromisso.Link,
            compromisso.Contato?.Id,
            compromisso.Contato?.Nome
        ));
    }

    public List<CompromissoDto> SelecionarTodos()
    {
        return repositorioCompromisso
            .SelecionarTodos()
            .Select(c => new CompromissoDto(
                c.Id,
                c.Assunto,
                c.DataOcorrencia,
                c.HoraInicio,
                c.HoraTermino,
                c.Tipo,
                c.Local,
                c.Link,
                c.Contato?.Id,
                c.Contato?.Nome
            ))
            .ToList();
    }
}
