using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Application.DTO.Token
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
