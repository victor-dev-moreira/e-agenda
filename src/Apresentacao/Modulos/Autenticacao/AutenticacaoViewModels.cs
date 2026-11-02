using System.ComponentModel.DataAnnotations;

public sealed record CadastroUsuarioViewModel(
    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email deve seguir um formato válido.")]
    [StringLength(256, ErrorMessage = "O email deve seguir um formato válido.")]
    string Email,

    [Required(ErrorMessage = "A senha é obrigatória")]
    [DataType(DataType.Password)]
    string Senha
)
{
    [Required(ErrorMessage = "A senha é obrigatória")]
    [DataType(DataType.Password)]
    [Compare(nameof(Senha), ErrorMessage = "As senhas não conferem.")]
    public string ConfirmarSenha { get; set; } = string.Empty;
};

public sealed record LoginUsuarioViewModel(
    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email deve seguir um formato válido.")]
    [StringLength(256, ErrorMessage = "O email deve seguir um formato válido.")]
    string Email,

    [Required(ErrorMessage = "A senha é obrigatória")]
    [DataType(DataType.Password)]
    string Senha,
    bool LembrarMe,
    string? ReturnUrl
);
