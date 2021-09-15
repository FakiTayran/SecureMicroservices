﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movies.API.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Text;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityServer4.AccessTokenValidation;

namespace Movies.API
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movies.API", Version = "v1" });
            });

            services.AddDbContext<MoviesAPIContext>(options =>
                    options.UseInMemoryDatabase("Movies"));

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
             .AddIdentityServerAuthentication(options =>
             {
                 options.Authority = "https://localhost:5005";
                 options.ApiName = "movieapi";
                 options.ApiSecret = "scopesecret";
                 options.RequireHttpsMetadata = false;
             });
            //services.AddAuthentication("Bearer")
            //.AddOAuth2Introspection("Bearer", options =>
            // {
            //     options.Authority = "https://localhost:5005";
            //     options.ClientId = "movieClient2";
            //     options.ClientSecret = "secret2";
            // });
            services.AddAuthorization();

            //services.AddAuthorization(x =>
            //{
            //    x.AddPolicy("read", policy => policy.RequireClaim("scope", "movieapi.read"));
            //    x.AddPolicy("write", policy => policy.RequireClaim("scope", "movieapi.write"));
            //});

            //services
            //    .AddAuthentication("Bearer")
            //    .AddJwtBearer("Bearer", options =>
            //{
            //    options.Authority = "https://localhost:5005";
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateAudience = false
            //    };
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ClientIdPolicy", policy => policy.RequireClaim("client_id","movieClient","","",""));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movies.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}