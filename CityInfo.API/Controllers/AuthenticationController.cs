using CityInfo.Shared.Models;
using CItyInfo.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInfo.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    //not adding authorize as it is needed for unauthorised users to get authorization
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationFacade _authenticationFacade;

        public AuthenticationController(IAuthenticationFacade authenticationFacade)
        {
            _authenticationFacade = authenticationFacade ?? 
                throw new ArgumentNullException(nameof(authenticationFacade));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(
            AuthenticationRequestBody authenticationRequestBody)
        {
            var tokenToReturn = _authenticationFacade.Authenticate(authenticationRequestBody);

            if (string.IsNullOrEmpty(tokenToReturn)) 
            {
                return NotFound();
            }

            return Ok(tokenToReturn);
        }
    }
}
