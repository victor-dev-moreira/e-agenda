using System.Security.Claims;
using eAgenda.Dominio.Compartilhado;

namespace eAgenda.WebApp.Modulos.Autenticacao;

public sealed class ProvedorUsuarioAtualHttp(IHttpContextAccessor httpContextAccessor) : IProvedorUsuarioAtual
{
    public Guid Id
    {
        get
        {
            string? contextId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool conseguiuConverter = Guid.TryParse(contextId, out Guid usuarioId);

            return conseguiuConverter ? usuarioId : Guid.Empty;
        }
    }
}
