using FerrumCapital.Application.DTO.Token;

namespace FerrumCapital.Application.Features.Commands.AppUser.Login
{
    public record LoginUserCommandResponse
    {
        public TokenDto Token { get; set; }
    }
}
