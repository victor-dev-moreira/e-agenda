using eAgenda.Dominio.Modulos.Compromissos;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

[ValidateNever]
public record CadastrarCompromissoViewModel(
    string Assunto,
    DateOnly DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso Tipo,
    string? Local,
    string? Link,
    Guid? ContatoId
)
{
    public List<OpcaoContatoViewModel> Contatos = [];
};

[ValidateNever]
public record EditarCompromissoViewModel(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso Tipo,
    string? Local,
    string? Link,
    Guid? ContatoId
)
{
    public List<OpcaoContatoViewModel> Contatos = [];
};

public record ExcluirCompromissoViewModel(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso Tipo,
    string? Local,
    string? Link,
    Guid? ContatoId,
    string? ContatoNome
);

public record OpcaoContatoViewModel(Guid Id, string Nome);
