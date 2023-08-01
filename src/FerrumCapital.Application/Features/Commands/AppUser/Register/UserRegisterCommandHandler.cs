using FerrumCapital.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FerrumCapital.Application.Features.Commands.AppUser.Register
{
    public record UserRegisterCommandHandler : IRequestHandler<UserRegisterCommandRequest, UserRegisterCommandResponse>
    {
        private readonly UserManager<FerrumCapital.Domain.Identity.AppUser> _userManager;

        public UserRegisterCommandHandler(UserManager<Domain.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserRegisterCommandResponse> Handle(UserRegisterCommandRequest request, CancellationToken cancellationToken)
        {
            var anyUser = await _userManager.FindByEmailAsync(request.Email);

            if (anyUser is not null) throw new UserAlreadyExistException("This email already exist");
            anyUser = await _userManager.FindByNameAsync(request.Username);
            if (anyUser is not null) throw new UserAlreadyExistException("This username already exist");

            Domain.Identity.AppUser newUser = new()
            {
                UserName = request.Username,
                Email = request.Email
            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, request.Password);
            if (!identityResult.Succeeded) throw new UserCreateFailedException("An unexpected error occurred while creating the user!");
            return new UserRegisterCommandResponse { IsSuccess = true };
        }
    }


}
