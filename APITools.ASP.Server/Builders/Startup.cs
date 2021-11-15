using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APITools.ASP.Server.Builders
{
    public abstract class Startup
    {
        public void Process(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            ConfigureServiceLocator(builder.Configuration);
            ConfigureApplicationBuilder(builder.Services, builder.Configuration);
            WebApplication app = builder.Build();
            ConfigureApplication(app, app.Environment);
            app.Run();
        }

        protected abstract void ConfigureServiceLocator(IConfiguration configuration);

        protected abstract void ConfigureApplicationBuilder(IServiceCollection services, IConfiguration configuration);

        protected abstract void ConfigureApplication(IApplicationBuilder app, IWebHostEnvironment env);
    }
}
