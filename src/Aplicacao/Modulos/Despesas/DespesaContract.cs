using eAgenda.Dominio.Modulos.Despesas;
namespace eAgenda.Aplicacao.Modulo.Despesas;

public record CategoriaDespesaDto(Guid Id, string Titulo);
public record DespesaDto(
    Guid Id,
    string Descricao,
    DateOnly DataOcorrencia,
    decimal Valor,
    FormaPagamento FormaPagamento,
    List<CategoriaDespesaDto> Categorias
);

public record CadastrarDespesaDto(
    string Descricao,
    DateOnly DataOcorrencia,
    decimal Valor,
    FormaPagamento FormaPagamento,
    List<Guid> CategoriaIds
);
public record EditarDespesaDto(
    Guid Id,
    string Descricao,
    DateOnly DataOcorrencia,
    decimal Valor,
    FormaPagamento FormaPagamento,
    List<Guid> CategoriaIds
);
