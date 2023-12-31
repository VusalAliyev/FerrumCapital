﻿using FerrumCapital.Application.Common.Security.Jwt;
using FerrumCapital.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FerrumCapital.Application.Features.Commands.AppUser.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly ITokenHandler _tokenHandler;
        private readonly UserManager<FerrumCapital.Domain.Identity.AppUser> _userManager;

        public LoginUserCommandHandler(ITokenHandler tokenHandler, UserManager<Domain.Identity.AppUser> userManager)
        {
            _tokenHandler = tokenHandler;
            _userManager = userManager;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
            if (user is null)
            {
                user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
                if (user is null) throw new NotFoundException();
            }
            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result) throw new AuthenticationErrorException();
            var accessToken = _tokenHandler.CreateAccessToken(20, user);

            return new LoginUserCommandResponse()
            {
                Token = accessToken
            };

        }
    }
}
