using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Destructurers;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Exceptions.SqlServer.Destructurers;
using Serilog.Exceptions;
using DistributedECommerce.Warehouse.API.Extensions;
using DistributedECommerce.Warehouse.API.Middleware;
using DistributedECommerce.Warehouse.API.Services;
using DistributedECommerce.Warehouse.Infrastructure.Data;
using DistributedECommerce.Warehouse.Application.Common.Infrastructure;
using DistributedECommerce.Warehouse.Infrastructure;
using DistributedECommerce.Warehouse.Application;
using DistributedECommerce.Warehouse.Application.BackgroundServices;
using DistributedECommerce.Warehouse.Application.Configurations;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Integration tests fail without preserveStaticLogger: true
    builder.Host.UseSerilog((ctx, lc) => lc
            .WriteTo.Console()
            .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                .WithDefaultDestructurers()
                .WithDestructurers(new ExceptionDestructurer[] { new SqlExceptionDestructurer(), new DbUpdateExceptionDestructurer() })
            )
            .Enrich.FromLogContext()
            .Enrich.With<ActivityEnricher>()
            .ReadFrom.Configuration(ctx.Configuration),
        preserveStaticLogger: true);

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddAPIServices();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddMessageQueue(builder.Configuration);

    var rabbitMqConfiguration = new RabbitMqConfiguration();
    builder.Configuration.Bind("RabbitMqConfiguration", rabbitMqConfiguration);
    builder.Services.AddSingleton(rabbitMqConfiguration);
    builder.Services.AddHostedService<OrderCreatedConsumer>();
    builder.Services.AddHostedService<OrderCanceledConsumer>();
    builder.Services.AddHostedService<ComponentStateChangedConsumer>();

    builder.Services
        .AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
        })
        .AddApiExplorer(options =>
        {
            // Add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        });

    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    var app = builder.Build();

    app.UseMiddleware<RequestLoggingMiddleware>();
    app.UseMiddleware<ExceptionMiddleware>();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
    await db.Database.MigrateAsync();

    var currentUserService = scope.ServiceProvider.GetRequiredService<ICurrentUserService>();
    await new WarehouseSeeder(db, currentUserService).SeedDefaultData();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception while bootstrapping application");
}
finally
{
    Log.Information("Shutting down...");
    Log.CloseAndFlush();
}
