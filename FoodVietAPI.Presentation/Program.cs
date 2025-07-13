using CleanFoodVietAPI.Presentation.Extensions;
using CleanFoodVietAPI.Presentation.Middlewares;

// using CleanFoodVietAPI.Presentation.Middlewares;   // no longer needed
// using Hangfire;
// using Hangfire.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// 1. Services registration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Registers your OpenAPI generator
builder.Services.AddOpenApi("v1", opts =>
    opts.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

builder.Services.AddCorsConfig();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddUnitOfWork();
builder.Services.AddJwtAuthenticationScheme(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddConfigSwagger();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Hangfire and MySQL (commented out for production; enable for local fun)
// builder.Services.AddHangfire(cfg =>
//     cfg.UseStorage(new MySqlStorage(
//         builder.Configuration.GetConnectionString("DefaultConnection")!,
//         new MySqlStorageOptions
//         {
//             TablesPrefix               = "Hangfire",
//             PrepareSchemaIfNecessary   = true,
//             QueuePollInterval          = TimeSpan.FromSeconds(15),
//             TransactionTimeout         = TimeSpan.FromMinutes(1),
//             JobExpirationCheckInterval = TimeSpan.FromMinutes(5),
//             CountersAggregateInterval  = TimeSpan.FromMinutes(5),
//             DashboardJobListLimit      = 10000
//         })));
// builder.Services.AddHangfireServer();

// Logging
builder.Logging
    .ClearProviders()
    .AddConsole()
    .AddAzureWebAppDiagnostics()
    // .AddFilter("Hangfire", LogLevel.Debug)   // only if Hangfire is in use
    ;

// build the app
var app = builder.Build();

// 2. Global and custom middleware
app.UseMiddleware<GlobalException>();
// app.UseMiddleware<ReconcileMiddleware>();   // removed in favor of scheduled job

// 3. HTTPS, Routing, CORS, Auth
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("Default");
app.UseAuthentication();
app.UseAuthorization();

// 4. OpenAPI endpoints + Swagger UI
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

// 5. Hangfire Dashboard (local only; comment for prod)
// app.UseHangfireDashboard("/hangfire");

// 6. Top level route registrations
app.MapControllers();
app.MapScalarApiReference(o =>
    o.WithTitle("Template API")
     .WithTheme(ScalarTheme.BluePlanet)
     .WithDarkMode(true));

// Manual trigger endpoint for heartbeat (local only)
// app.MapGet("/hangfire/trigger-heartbeat", (IRecurringJobManager mgr) =>
// {
//     mgr.Trigger("heartbeat-2mins");
//     return Results.Ok("Heartbeat job triggered");
// });

// 7. Recurring jobs (outside of HTTP pipeline; local only)
// RecurringJob.AddOrUpdate<ExpireContractsJob>(
//     "expire-contracts",
//     job => job.ExecuteAsync(),
//     Cron.MinuteInterval(15),
//     new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
// RecurringJob.AddOrUpdate(
//     "heartbeat-2min",
//     () => Console.WriteLine($"[Hangfire] Heartbeat at {DateTime.UtcNow:O}"),
//     "*/2 * * * *"
// );

// 8. Start the host
app.Run();