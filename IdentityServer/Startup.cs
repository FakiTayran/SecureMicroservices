using AspNetCore.Identity.Dapper;
using AspNetCore.Identity.Dapper.Models;
using AspNetCore.Identity.DatabaseScripts.DbUp;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Dapper.Storage;
using IdentityServer4.Dapper.Storage.DataLayer;
using IdentityServer4.Models;
using IdentityServer4.Storage.DatabaseScripts.DbUp;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString(name: "DefaultConnection");


            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            })
                  .AddDapperStores(options =>
                  {
                      options.ConnectionString = connectionString;
                      options.DbSchema = "dbo";
                  });

            services.AddIdentityDbUpDatabaseScripts(options => {
                options.ConnectionString = connectionString;
                options.DbSchema = "dbo";
            });

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.IssuerUri ="https://localhost:5005";
            })
                .AddAspNetIdentity<ApplicationUser>()
                .AddDeveloperSigningCredential()
                .AddInMemoryClients(Config.Clients)
                .AddInMemoryApiResources(Config.ApiResource)
                .AddInMemoryApiScopes(Config.ApiScopes).AddDbUpDatabaseScripts(options =>
                {
                    options.ConnectionString = connectionString;
                    options.DbSchema = "dbo";

                })
                .AddSQLConnection(option => { option.ConnectionString = connectionString; option.DbSchema = $"[dbo]"; })
                .AddDapperConfigurationStore()

                .AddDapperOperationalStore(option =>
                {
                    option.EnableTokenCleanup = true;
                    option.TokenCleanupInterval = 3600;
                });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
             .AddIdentityServerAuthentication(options =>
             {
                 options.Authority = "https://localhost:5005";
                 options.ApiName = "movieapi";
                 options.ApiSecret = "scopesecret";
                 options.RequireHttpsMetadata = false;
             });
            services.AddAuthorization();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IIdentityServerMigrations identityServerMigrations, IClientProvider _clientProvider,IApiResourceProvider _apiResourceProvider, IApiScopesProvider _apiScopesProvider)
        {
            //var connectionString = Configuration.GetConnectionString(name: "DefaultConnection");
            //var identityMigrations = new AspNetCore.Identity.DatabaseScripts.DbUp.Migrations(connectionString, "dbo");
            //var identityResult = identityMigrations.UpgradeDatabase();
            identityServerMigrations.UpgradeDatabase(true);
            _clientProvider.AddAsync(new Client
            {
                ClientId = "movieclient2",
                ClientName = "movieApi2",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // it is used by clients to obtain an access token outside of the context of a user
                AllowedScopes = {
                         "movieapi.read",
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile
                     },
                AccessTokenType = AccessTokenType.Reference,
                RequireConsent = false,
                RequireClientSecret = false,
                
            });
            //_apiResourceProvider.AddAsync(new ApiResource("movieapi")
            //{
            //    Scopes = new List<string> { "movieapi.read", "movieapi.write" },
            //    ApiSecrets = new List<Secret> { new Secret("scopesecret".Sha256()) }, //client aware of this value
            //    UserClaims = new List<string> { "role" }
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server.API v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
