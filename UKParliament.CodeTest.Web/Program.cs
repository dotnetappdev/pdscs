using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Repositories;
using UKParliament.CodeTest.Services.Validation;
using UKParliament.CodeTest.Web.Mapper;
using Serilog;
using FluentValidation.AspNetCore;
namespace UKParliament.CodeTest.Web;

public class Program
{
    public static void Main(string[] args)
    {

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File(
                path: "logs/{Year}/{Month}/{Day}.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        // Replace default logging with Serilog
        builder.Host.UseSerilog();

        // Register Serilog.ILogger for DI
        builder.Services.AddSingleton(Log.Logger);

        // Add services to the container.

        builder.Services.AddControllersWithViews()
      .AddJsonOptions(options =>
      {
          options.JsonSerializerOptions.PropertyNamingPolicy = null;
      });
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<PersonValidator>();

        builder.Services.AddDbContext<PersonManagerContext>(op => op.UseInMemoryDatabase("PersonManager"));

        builder.Services.AddScoped<IPersonReadService<Person>, PersonReadRepository>();
        builder.Services.AddScoped<IPersonWriteService<Person>, PersonWriteRepository>();
        builder.Services.AddScoped<PersonReadService>();
        builder.Services.AddScoped<PersonWriteService>();
        builder.Services.AddScoped<PersonMapperBase, PersonMapper>();
        builder.Services.AddScoped<IDepartmentReadRepository, DepartmentReadService>();
        builder.Services.AddScoped<IDepartmentWriteRepository, DepartmentWriteService>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // Enable Swagger middleware
        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        // Move seeding here, after app is built and before app.Run
        using (var serviceScope = app.Services.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<PersonManagerContext>();
            context.Database.EnsureCreated();
            DbSeeder.SeedInMemoryData(context);
            Console.WriteLine($"Seeded people count: {context.People.Count()}");
        }

        app.Run();
    }
}