using AspNetIdentity.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentity.WebApi.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AspNetIdentity.WebApi.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AspNetIdentity.WebApi.Infrastructure.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "SuperPowerUser",
                Email = "keith.smith@kssoftware.net",
                EmailConfirmed = true,
                FirstName = "Keith",
                LastName = "Smith",
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3)
            };

            manager.Create(user, "0icu*12");
        }
    }
}
