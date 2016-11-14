using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NomadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebAPI.Exceptions;
using WebAPI.Services;

namespace WebAPI.Filters
{
    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
    {
        protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            UserManager<User> userManager = CreateUserManager();

            cancellationToken.ThrowIfCancellationRequested(); // Unfortunately, UserManager doesn't support CancellationTokens.
            var email = userName;
            User user = await userManager.FindAsync(email, password);

            if (user == null)
            {
                // No user with userName/password exists.
                return null;
            }
			
            // Create a ClaimsIdentity with all the claims for this user.
            cancellationToken.ThrowIfCancellationRequested(); // Unfortunately, IClaimsIdenityFactory doesn't support CancellationTokens.
            ClaimsIdentity identity = await userManager.ClaimsIdentityFactory.CreateAsync(userManager, user, "Basic");
            
            return new ClaimsPrincipal(identity);
        }

        private static UserManager<NomadModel.User> CreateUserManager()
        {
            //return new UserManager<NomadModel.User>(new UserStore<NomadModel.User>(new NomadModel.NomadContext()));
            return new UserManager<NomadModel.User>(new UserStoreService()) { PasswordHasher = new PasswordStorage() };
        }
    }
}