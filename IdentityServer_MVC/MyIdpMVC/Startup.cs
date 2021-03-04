using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyIdp.Data;
using MyIdp.Services;
using MyIdp.Validator;
using System.Linq;
using System.Reflection;

namespace MyIdp
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<UserDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:UserDbConnection"].ToString()));

            if (Environment.IsProduction())
            {
                //var connectionString = Configuration["ConnectionStrings:IdentityServerConnection"].ToString();
                //services.AddIdentityServer(options =>
                //{
                //    options.Events.RaiseErrorEvents = true;
                //    options.Events.RaiseInformationEvents = true;
                //    options.Events.RaiseFailureEvents = true;
                //    options.Events.RaiseSuccessEvents = true;
                //    options.EmitStaticAudienceClaim = true;
                //})
                // // this adds the config data from DB (clients, resources)
                //.AddConfigurationStore(options =>
                //{
                //    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                //        sql => sql.MigrationsAssembly("MyIdp"));
                //})
                // // this adds the operational data from DB (codes, tokens, consents)
                //.AddOperationalStore(options =>
                //{
                //    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                //        sql => sql.MigrationsAssembly("MyIdp"));
                //})
                //.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                //.AddProfileService<CustomProfileService>()
                //.AddSigningCredential(new X509Certificate2("","");
            }
            else
            {
                services.AddIdentityServer()
                   .AddInMemoryClients(Config.Clients)
                   .AddInMemoryApiScopes(Config.ApiScopes)
                   .AddInMemoryIdentityResources(Config.IdentityResources)
                   .AddExtensionGrantValidator<PhoneNumberGrandValidator>()
                   .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                   .AddProfileService<UserProfileService>()
                   .AddDeveloperSigningCredential();
            }

            services.AddAuthentication()
                .AddGitHub(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = "d29ef499788464e860ce";
                    options.ClientSecret = "4ad25c5470d1045c5ff4b1fb3bd5a3bc70444aeb";
                    options.Scope.Add("user:email");
                });

            services.AddScoped<ISysUserService, SysUserService>();
            services.AddScoped<ISmsService, SmsService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            //InitializeDatabase(app);

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }


        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}