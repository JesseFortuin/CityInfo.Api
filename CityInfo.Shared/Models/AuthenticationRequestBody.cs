using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.Shared.Models
{
    public class AuthenticationRequestBody
    {
        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}
