using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using neigh.Models;

[assembly: OwinStartupAttribute(typeof(neigh.Startup))]
namespace neigh
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
        }

        private void CreateRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (!roleManager.RoleExists("Admin"))
            {
                var newRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                newRole.Name = "Admin";
                roleManager.Create(newRole);
            }

            if (!userManager.IsInRole("b7687cff-a0b2-43ed-892f-624317dbdcf7", "Admin"))
            {
                userManager.AddToRole("b7687cff-a0b2-43ed-892f-624317dbdcf7", "Admin");
            }
            if (!userManager.IsInRole("dbcfcaf0-622c-489b-b94c-0d9543a7b5e2", "Admin"))
            {
                userManager.AddToRole("dbcfcaf0-622c-489b-b94c-0d9543a7b5e2", "Admin");
            }
        }
    }
}
