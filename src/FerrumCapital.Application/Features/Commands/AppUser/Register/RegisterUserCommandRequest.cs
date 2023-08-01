using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Application.Features.Commands.AppUser.Register
{
    public record UserRegisterCommandRequest : IRequest<UserRegisterCommandResponse>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
