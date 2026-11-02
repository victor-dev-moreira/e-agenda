using eAgenda.Dominio.Modulos.Compromissos;

namespace eAgenda.Aplicacao.Modulos.Compromissos;

public record CompromissoDto(
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


public record CadastrarCompromissoDto(
    string Assunto,
    DateOnly DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso Tipo,
    string? Local,
    string? Link,
    Guid? ContatoId
);

public record EditarCompromissoDto(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso Tipo,
    string? Local,
    string? Link,
    Guid? ContatoId
);

public record OpcaoContatoDto(Guid Id, string Nome);
