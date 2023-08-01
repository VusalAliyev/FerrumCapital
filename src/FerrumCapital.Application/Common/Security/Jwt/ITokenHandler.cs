using FerrumCapital.Application.DTO.Token;
using FerrumCapital.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Application.Common.Security.Jwt
{
    public interface ITokenHandler
    {
        TokenDto CreateAccessToken(int second, AppUser appUser);
    }
}
