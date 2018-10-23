using AspNetCore.Sample.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Sample
{
  public class Startup
  {
    private readonly IHostingEnvironment _hostingEnvironment;

    public Startup(IHostingEnvironment env)
    {
      _hostingEnvironment = env;
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      // AUTENTHICATION  **************************************************
      // uncomment if needed
      //services.AddAuthentication(sharedOptions =>
      //    {
      //      sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      //      sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
      //    })
      //    .AddAzureAd(options =>
      //    {
      //      Configuration.Bind("AzureAd", options);
      //      AzureAdOptions.Settings = options;
      //    })
      //    .AddAzureAdBearer(options => Configuration.Bind("AzureAd", options))
      //    .AddCookie();

      services.AddMvc().AddSessionStateTempDataProvider();
      services.AddSession();

      // DATABASE **************************************************
      // the local connection string is stored in appsettings.json
      // the production one should be set as an environment variable 
      // Run these commands to set up the db:
      // Add-Migration init
      // Update-Database
      services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Local")));


    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        // app.UseBrowserLink();
        app.UseDeveloperExceptionPage();
        app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
        {
          HotModuleReplacement = true
        });
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();

      app.UseSession(); // Needs to be app.UseAuthentication() and app.UseMvc() otherwise you will get an exception "Session has not been configured for this application or request."
      app.UseAuthentication();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");

        routes.MapSpaFallbackRoute(
                  name: "spa-fallback",
                  defaults: new { controller = "Home", action = "Index" });
      });
    }
  }
}
