using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace AspNetIdentity.WebApi.Infrastructure
{
    public class ApplicationUserManger : UserManager<ApplicationUser>
    {
        public ApplicationUserManger(IUserStore<ApplicationUser> store) : base(store)
        {
        }

        public static ApplicationUserManger Create(IdentityFactoryOptions<ApplicationUserManger> options,
            IOwinContext context)
        {
            var appDbContext = context.Get<ApplicationDbContext>();
            var appUserManager = new ApplicationUserManger(new UserStore<ApplicationUser>(appDbContext));

            appUserManager.EmailService = new AspNetIdentity.WebApi.Services.EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
                appUserManager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.Net Identity"))
                    {
                        TokenLifespan = TimeSpan.FromHours(6)
                    };
            appUserManager.UserValidator = new UserValidator<ApplicationUser>(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            appUserManager.PasswordValidator = new PasswordValidator()
            {
                RequireDigit = false,
                RequireLowercase = true,
                RequireNonLetterOrDigit = true,
                RequireUppercase = true,
                RequiredLength = 6
            };

            return appUserManager;
        }
    }
}