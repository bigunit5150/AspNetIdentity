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
            return appUserManager;
        }
    }
}