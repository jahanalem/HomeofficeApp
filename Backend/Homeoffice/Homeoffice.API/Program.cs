using Homeoffice.Contracts.Configurations;
using Homeoffice.Contracts.Services;
using Homeoffice.DataAccess;
using Homeoffice.Models.Entities.Identity;
using Homeoffice.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;


try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

    // Database context configurieren
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    // Identity Core
#pragma warning disable IL2026
    builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        // Hier können Sie Passwort-Regeln etc. anpassen
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();
#pragma warning restore IL2026

    // JWT-authentication configuration
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateIssuer = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateAudience = true
        };
    });

    // Services
    builder.Services.AddScoped<ITimeTrackingService, TimeTrackingService>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();

    // API-Controller
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        // Prevents infinite loops for linked entities
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

    // CORS-Policy for the Angular frontend
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy.WithOrigins("http://localhost:4300")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });


    var app = builder.Build();

    app.UseCors("CorsPolicy");
    app.UseRouting();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await SeedDatabaseAsync(services);

    await app.RunAsync();
}
catch (Exception exception)
{
    Console.WriteLine($"Stopped program because of exception:{exception}");
    throw;
}

async Task SeedDatabaseAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    if (!await userManager.Users.AnyAsync())
    {
        // Create User 1
        var user1 = new User
        {
            UserName = "testuser",
            Email = "test@example.com",
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user1, "Pa$$word123");

        // Create User 2
        var user2 = new User
        {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(user2, "Pa$$word123");

        Console.WriteLine("Default users created successfully.");
    }
}
