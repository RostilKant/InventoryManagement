using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using Entities.DataTransferObjects.User;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<(bool isCreated, IEnumerable<IdentityError>)> RegisterOrganizationAsync(OrganizationForRegistrationDto organizationForRegistration);
        Task<(bool, IEnumerable<IdentityError>)> RegisterUserAsync(UserForRegistrationDto userForRegistration);
        Task<bool> AuthenticateUserAsync(string tenant, UserForAuthenticationDto userForAuthentication);
        Task<(string, DateTime)> CreateTokenAsync();
    }
}