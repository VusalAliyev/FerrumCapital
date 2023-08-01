using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Application.Features.Commands.AppUser.Login
{
    public record LoginUserCommandRequest : IRequest<LoginUserCommandResponse>
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
