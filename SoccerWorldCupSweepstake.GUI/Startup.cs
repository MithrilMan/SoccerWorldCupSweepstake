using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoccerWorldCupSweepstake.GUI.Models;

namespace SoccerWorldCupSweepstake.GUI {
   public class Startup {
      public Startup(IConfiguration configuration) {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services) {
         services.AddMvc();
         services.AddSpaStaticFiles(c => {
            c.RootPath = "ClientApp/dist";
         });
         services.Configure<SmartContractConfiguration>(options => Configuration.GetSection("SmartContractConfiguration").Bind(options));

      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
         if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
         }


         app.UseMvc();


         app.UseStaticFiles();
         app.UseSpaStaticFiles();
         app.UseMvc(routes => {
            routes.MapRoute(name: "default", template: "{controller}/{action=index}/{id}");
         });

         app.UseSpa(spa => {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501

            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment()) {
               spa.UseAngularCliServer(npmScript: "start");
            }
         });
      }
   }
}
