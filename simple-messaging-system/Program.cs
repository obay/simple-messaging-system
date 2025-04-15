using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using simple_messaging_system.Data;
using Serilog;
using Serilog.Events;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

public class Program
{
    public static void Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Starting web application");
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Configure XPO
            XpoDataStore.Instance.GetDataLayer();

            // Use Serilog for logging
            builder.Host.UseSerilog();

            var app = builder.Build();

            // Add request logging middleware
            app.Use(async (context, next) =>
            {
                var startTime = Stopwatch.GetTimestamp();
                var request = context.Request;
                
                // Log request details
                Log.Information("HTTP {Method} {Path} started", request.Method, request.Path);

                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Unhandled exception occurred while processing {Method} {Path}", 
                        request.Method, request.Path);
                    throw;
                }
                finally
                {
                    var response = context.Response;
                    var elapsedMs = Stopwatch.GetElapsedTime(startTime).TotalMilliseconds;
                    
                    // Log response details
                    Log.Information("HTTP {Method} {Path} completed with {StatusCode} in {ElapsedMs}ms",
                        request.Method, request.Path, response.StatusCode, elapsedMs);
                }
            });

            // Configure the HTTP request pipeline.
            // Enable Swagger by default unless DISABLE_SWAGGER environment variable is set to "true"
            bool disableSwagger = string.Equals(Environment.GetEnvironmentVariable("DISABLE_SWAGGER"), "true", StringComparison.OrdinalIgnoreCase);
            if (!disableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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
