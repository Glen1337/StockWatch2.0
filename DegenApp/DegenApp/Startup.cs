using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
using DegenApp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Alpaca.Markets;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using DegenApp.Auth;
using Microsoft.AspNetCore.Authorization;
using DegenApp.Scraper;
using IEXSharp;

namespace DegenApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string domain = "";
            
            //if(_env.IsDevelopment())
            //{
                domain = $"https://{Configuration["Auth0:Domain"]}/";
            //}

            //if (_env.IsDevelopment())
            //{
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            //}

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = Configuration["Auth0:Audience"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("create:data", policy => policy.Requirements.Add(new HasScopeRequirement("create:data", domain)));
                options.AddPolicy("edit:data", policy => policy.Requirements.Add(new HasScopeRequirement("edit:data", domain)));
                options.AddPolicy("read:data", policy => policy.Requirements.Add(new HasScopeRequirement("read:data", domain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddCors(options =>
            {

                options.AddPolicy(MyAllowSpecificOrigins,
                builder => builder.AllowAnyMethod()
                            .AllowAnyHeader()
                            .SetIsOriginAllowed(host => true)
                            .AllowCredentials());
               });

            services.AddControllers()
                // set to false in prod
                .AddJsonOptions(opts => opts.JsonSerializerOptions.WriteIndented = true)
                .AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // To allow API test tokens to be used in swagger to call API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DegenApp", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    //Type = SecuritySchemeType.Http,
                    //Description = "Please insert JWT with Bearer into field like: \"Authorization: Bearer {token}\"",
                    Description = "Please insert JWT with Bearer into field like: Bearer {token}. No quotes!",
                    Name = "Authorization"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                   {
                     new OpenApiSecurityScheme() { Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                     new string[] { }
                    }
                 });
            });

            //services.AddSingleton<IOptionScraper, OptionScraper>();

            services.AddSingleton<IAlpacaDataClient>(adc =>
            {
                return Alpaca.Markets.Environments.Paper.GetAlpacaDataClient(new SecretKey(Configuration["Alpaca:Key"], Configuration["Alpaca:Secret"]));
            });

            services.AddSingleton<IAlpacaTradingClient>(atc =>
            {
                return Alpaca.Markets.Environments.Paper.GetAlpacaTradingClient(new SecretKey(Configuration["Alpaca:Key"], Configuration["Alpaca:Secret"]));
            });

            services.AddSingleton<IEXCloudClient>(iexc =>
            {
                return new IEXCloudClient(Configuration["IEX:PublishableToken"], Configuration["IEX:SecretToken"], false, false);
            });

            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DegenApp v1"));
            }

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
