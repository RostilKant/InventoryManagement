using System.Threading.Tasks;
using Entities.DataTransferObjects.User;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterUserAsync(UserForRegistrationDto userForRegistration, ModelStateDictionary modelState);
        Task<bool> AuthenticateUserAsync(UserForAuthenticationDto userForAuthentication);
        Task<string> CreateTokenAsync();
    }
}