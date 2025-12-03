using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PlayZone.Data;
using PlayZone.Configuration;
using PlayZone.Repositories;
using PlayZone.Services;
using PlayZone.Utilities;
using PlayZone.Middleware;
using Serilog;

namespace PlayZone;

public class Program
{
    public static void Main(string[] args)
  {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
       .WriteTo.File("logs/playzone-.txt", rollingInterval: RollingInterval.Day)
 .CreateLogger();

        try
        {
        Log.Information("Starting PlayZone API");

        var builder = WebApplication.CreateBuilder(args);

   // Add Serilog
      builder.Host.UseSerilog();

        // Load configuration
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
            var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>()!;

      // Add services to the container
            builder.Services.AddControllers();

     // Configure DbContext with PostgreSQL
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
 builder.Services.AddDbContext<PlayZoneDbContext>(options =>
        options.UseNpgsql(connectionString));

     // Register Repositories
     builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
     builder.Services.AddScoped<IBlockedSlotRepository, BlockedSlotRepository>();
          builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IWhatsAppConfigRepository, WhatsAppConfigRepository>();

// Register Services
    builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
          builder.Services.AddScoped<IBlockedSlotService, BlockedSlotService>();
     builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services.AddScoped<IWhatsAppService, WhatsAppService>();

          // Register Utilities
   builder.Services.AddScoped<IValidationService, ValidationService>();

       // Configure JWT Authentication
        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
          builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
 .AddJwtBearer(options =>
  {
        options.RequireHttpsMetadata = false;
                options.SaveToken = true;
           options.TokenValidationParameters = new TokenValidationParameters
       {
              ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(key),
           ValidateIssuer = true,
     ValidIssuer = jwtSettings.Issuer,
           ValidateAudience = true,
             ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
             ClockSkew = TimeSpan.Zero
           };
            });

       // Configure CORS
          builder.Services.AddCors(options =>
            {
           options.AddPolicy("AllowFrontend", policy =>
         {
     policy.WithOrigins(corsSettings.AllowedOrigins)
             .AllowAnyMethod()
     .AllowAnyHeader()
         .AllowCredentials();
     });
 });

    // Register Configuration classes
          builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.Configure<AdminSettings>(builder.Configuration.GetSection("AdminSettings"));
       builder.Services.Configure<WhatsAppSettings>(builder.Configuration.GetSection("WhatsAppSettings"));
  builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection("CorsSettings"));
 builder.Services.Configure<BusinessSettings>(builder.Configuration.GetSection("BusinessSettings"));

            // Swagger/OpenAPI - FIXED
            builder.Services.AddEndpointsApiExplorer();
          builder.Services.AddSwaggerGen(c =>
   {
      c.SwaggerDoc("v1", new() { Title = "PlayZone API", Version = "v1" });

        // Add JWT authentication to Swagger - FIXED VERSION
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
     {
         Description = "JWT Authorization header using the Bearer scheme. Enter your token in the text input below.",
            Name = "Authorization",
   In = Microsoft.OpenApi.Models.ParameterLocation.Header,
          Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
              Scheme = "bearer",
        BearerFormat = "JWT"
 });

         c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
       {
           {
               new Microsoft.OpenApi.Models.OpenApiSecurityScheme
     {
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
     {
             Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
          Id = "Bearer"
                 }
      },
      Array.Empty<string>()
      }
      });
        });

      var app = builder.Build();

       // Run database migrations automatically
            using (var scope = app.Services.CreateScope())
   {
 var db = scope.ServiceProvider.GetRequiredService<PlayZoneDbContext>();
       try
       {
  db.Database.Migrate();
  Log.Information("Database migrations applied successfully");
    }
                catch (Exception ex)
    {
   Log.Error(ex, "An error occurred while migrating the database");
 }
        }

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
    {
 app.UseSwagger();
     app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

 // Use CORS
            app.UseCors("AllowFrontend");

         // Use custom error handling middleware
          app.UseErrorHandling();

          // Use Serilog request logging
  app.UseSerilogRequestLogging();

    app.UseAuthentication();
            app.UseAuthorization();

         app.MapControllers();

            app.Run();
      }
        catch (Exception ex)
  {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
 {
    Log.CloseAndFlush();
        }
    }
}
