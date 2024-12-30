using Microsoft.EntityFrameworkCore;
using Squib.UserService.API;
using Squib.UserService.API.ChatApp;
using Squib.UserService.API.Profile;
using Squib.UserService.API.Repository;
using Squib.UserService.API.Service;
using Squib.UserService.API.DB;
// Ensure to include the Service namespace

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR();
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        // Add Redis distributed cache configuration
        // services.AddDistributedRedisCache(options =>
        // {
        //     options.Configuration = "localhost:6379"; // Ensure this points to your Redis server
        // });

        // Add CORS policy for cross-origin requests
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                .WithOrigins("http://localhost:5173") // Ensure this matches the URL of your client app
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .SetIsOriginAllowed((host) => true) // This allows all origins, adjust for production
                   .AllowCredentials();
            });
        });

        // Register SignalR with detailed error logging and connection options


        // Add controllers for the API
        services.AddControllers();

        // Register AutoMapper
        services.AddAutoMapper(typeof(UserProfile).Assembly);

        // Register your repository (UserRepo) for dependency injection
        services.AddScoped<IUserRepo, UserRepo>();

        // Register your service (UserServi) for dependency injection
        services.AddScoped<IUSER_Service, UserServi>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        context.Database.Migrate(); // Apply any pending migrations

        // Ensure the app uses HTTPS redirection and routing
        app.UseHttpsRedirection();
        app.UseWebSockets(new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromSeconds(120),
        });

        app.UseRouting();

        // Enable authorization if needed
        app.UseAuthorization();

        // Apply the CORS policy
        app.UseCors("CorsPolicy");

        // Configure the endpoints for the controllers and SignalR hubs
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<ChatHub>("/chatHub");
            endpoints.MapHub<TrackingHub>("/trackingHub"); // SignalR hub endpoint
        });
    }
}
