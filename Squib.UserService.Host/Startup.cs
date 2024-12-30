using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Squib.UserService.API;
using Squib.UserService.API.Profile;
using Squib.UserService.API.Repository;
using Squib.UserService.API.DB; // Ensure this is included
using System.Text;
using System;
using Microsoft.EntityFrameworkCore;

namespace Squib.UserService.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // var dbHost = Environment.GetEnvironmentVariable("DB_HOST")+".default.svc.cluster.local";
            // var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            // var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            // var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
            Console.WriteLine($"Connection String: {connectionString}");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddLogging(opts =>
            {
                opts.ClearProviders();

                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .ReadFrom.Configuration(Configuration)
                    .CreateLogger();

                opts.AddSerilog(dispose: true);
            });

            services.AddSquibUserService(Configuration);

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddHealthChecks();

            // Add JWT Authentication
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(options =>
            //     {
            //         options.Authority = "http://localhost:5140"; // IdentityServer URL
            //         options.Audience = "api1";
            //         options.RequireHttpsMetadata = false;

            //         options.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuer = true,
            //             ValidateAudience = true,
            //             ValidateLifetime = true,
            //             ValidateIssuerSigningKey = true,
            //             ValidIssuer = "http://localhost:5140", // IdentityServer URL
            //             ValidAudience = "api1"
            //         };
            //     });

            services.AddAutoMapper(typeof(UserProfile).Assembly); // Adjust as necessary
            // services.AddScoped<IUserRepo, UserRepo>(); // Register your UserRepo
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            context.Database.Migrate(); // Apply any pending migrations

            app.UseHttpsRedirection();
            app.UseRouting();
            // app.UseAuthentication();
            // app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/livez");
                endpoints.MapControllers();
            });
        }
    }
}