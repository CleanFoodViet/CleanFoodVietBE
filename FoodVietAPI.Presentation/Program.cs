using CleanFoodVietAPI.Presentation.Extensions;
using CleanFoodVietAPI.Presentation.Middlewares;
using Microsoft.Extensions.Logging.AzureAppServices;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddStripeConfiguration(builder.Configuration);
builder.Services.AddOpenApi("v1",
    options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

#region DI Setup
builder.Services.AddCorsConfig();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddUnitOfWork();
builder.Services.AddJwtAuthenticationScheme(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddConfigSwagger();
#endregion

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Logging
    .ClearProviders()
    .AddConsole()
    .AddAzureWebAppDiagnostics(); // Ensure the required package is installeddotnet add package Microsoft.Extensions.Logging.AzureAppServices
var app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
app.MapScalarApiReference(options =>
    options
        .WithTitle("Template API")
        .WithTheme(ScalarTheme.BluePlanet)
        .WithDarkMode(true)
);

app.UseCors("Default");
app.UseMiddleware<GlobalException>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
