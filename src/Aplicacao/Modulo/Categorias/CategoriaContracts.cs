namespace eAgenda.Aplicacao.Modulo.Categorias;

// DTO = Data Transfer Objetct

public record CategoriaDto(
    Guid Id,
    string Titulo
);
public record CadastrarCategoriaDto(
    string Titulo
);

public record EditarCategoriaDto(
    Guid Id,
    string Titulo
);
