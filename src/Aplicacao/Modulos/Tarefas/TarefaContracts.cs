using eAgenda.Dominio.Modulos.Tarefas;
namespace eAgenda.Aplicacao.Modulo.Tarefas;

public record TarefaDto(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    DateOnly DataCriacao,
    DateOnly? DataConclusao,
    int PercentualConcluido,
    bool Concluida,
    List<ItemTarefaDto> Itens
);
public record CadastrarTarefaDto(
    string Titulo,
    PrioridadeTarefa Prioridade
);

public record EditarTarefaDto(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade
);

public record AdicionarItemTarefaDto(
    Guid TarefaId,
    string Titulo
);

public record AlterarConclusaoItemTarefaDto(
    Guid TarefaId,
    Guid ItemId,
    bool concluido
);

public record RemoverItemTarefaDto(
    Guid TarefaId,
    Guid ItemId
);
public record AdicionarItemTarefaView(
    Guid Id,
    string Titulo
);

public record ItemTarefaDto(
    Guid Id,
    string Titulo,
    bool Concluido
);
