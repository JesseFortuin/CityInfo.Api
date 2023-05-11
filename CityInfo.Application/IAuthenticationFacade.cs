using CityInfo.Shared.Models;

namespace CItyInfo.Application
{
    public interface IAuthenticationFacade
    {
        string Authenticate(AuthenticationRequestBody authenticationRequestBody);
    }
}
