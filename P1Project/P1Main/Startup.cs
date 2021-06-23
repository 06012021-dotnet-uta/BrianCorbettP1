using BusinessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P1Main
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      services.AddDistributedMemoryCache();
      services.AddSession();
      services.AddControllersWithViews();
      // this is dependency injection of the DbContext
      services.AddDbContext<P1Db>(options =>
      {
        if (!options.IsConfigured)
          options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); // saved in P1Main(r-click) > Manage user secrets
      });
      services.AddScoped<IDbInteract, DbInteract>();

      // AddScoped (lasts for as long as one http request)
      // AddTransient (shorter lifetime than scoped)
      // AddSingleton (lasts for as long as the program is running)
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Landing/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSession();
      app.UseRouting();

      //app.UseMvc(routes =>
      //{
      //  routes.MapRoute(
      //    name: "default",
      //    template: "{controller=Home}/{action=Index}/{id?}");
      //});

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Landing}/{action=Index}/{id?}");
      });
    }
  }
}
