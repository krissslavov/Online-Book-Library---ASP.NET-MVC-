using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using OnlineBookLibrary.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineBookLibrary.Startup))]
namespace OnlineBookLibrary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
        }

       
        
    }
}
