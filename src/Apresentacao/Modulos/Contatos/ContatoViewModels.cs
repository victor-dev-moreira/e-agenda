namespace eAgenda.WebApp.Modulos.Contatos;

public record CadastrarContatoViewModel(
    string Nome,
    string Email,
    string Telefone,
    string? Cargo,
    string? Empresa
);

public record EditarContatoViewModel(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string? Cargo,
    string? Empresa
);

public record ExcluirContatoViewModel(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string? Cargo,
    string? Empresa
);
