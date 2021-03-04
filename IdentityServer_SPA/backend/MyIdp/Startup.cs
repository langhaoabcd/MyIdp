using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyIdp.Data;
using MyIdp.Filter;
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
            services.AddControllers(x =>
                {
                    x.Filters.Add(typeof(ExceptionsFilter));
                });

            services.AddDbContext<UserDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:UserDbConnection"].ToString()));
            services.AddCors(x => x.AddPolicy("cors", policy => policy.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
            services.AddSameSiteCookiePolicy();

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
                //.AddConfigurationStore(options =>
                //{
                //    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                //        sql => sql.MigrationsAssembly("MyIdp"));
                //})
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
                services.AddIdentityServer(options =>
                {
                    options.Events.RaiseSuccessEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.MutualTls.Enabled = true;
                    options.MutualTls.DomainName = "mtls";
                    options.AccessTokenJwtType = "JWT";
                    options.Cors.CorsPaths.Add(new PathString("/connect/authorize"));
                    options.Cors.CorsPaths.Add(new PathString("/connect/authorize/callback"));
                    options.Cors.CorsPaths.Add(new PathString("/api/account/login"));
                    options.Cors.CorsPaths.Add(new PathString("/api/account/logout"));
                    options.UserInteraction.LoginUrl = "http://localhost:8080/#/login";
                    options.UserInteraction.LogoutUrl = "http://localhost:8080/#/logout";
                    options.UserInteraction.ErrorUrl = "http://localhost:8080/#/error";
                })
                   .AddInMemoryClients(Config.Clients)
                   .AddInMemoryApiScopes(Config.ApiScopes)
                   .AddInMemoryIdentityResources(Config.IdentityResources)
                   .AddExtensionGrantValidator<PhoneNumberGrandValidator>()
                   .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                   .AddProfileService<UserProfileService>()
                   .AddDeveloperSigningCredential();
            }

            services.AddAuthentication()
                .AddGitHub("GitHub", options =>
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

            app.UseCors("cors");
            app.UseCookiePolicy();

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