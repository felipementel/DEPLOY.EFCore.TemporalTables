using Asp.Versioning;
using DEPLOY.TemporalTables.API.Endpoints.v1;
using DEPLOY.TemporalTables.API.Endpoints.v2;
using DEPLOY.TemporalTables.API.Infra.Database.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.IncludeFields = true;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddRouting(opt =>
{
    opt.LowercaseUrls = true;
    opt.LowercaseQueryStrings = true;
});

builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(2, 0);
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";

        options.SubstituteApiVersionInUrl = true;
    })
    .EnableApiVersionBinding();

builder.Services.AddEndpointsApiExplorer();

//var itemConfig = builder.Services
//    .AddOptions<DatabaseSettings>()
//    .BindConfiguration("Database")
//    .ValidateDataAnnotations()
//    .ValidateOnStart()
//    .Validate(config =>
//    {
//        if (config is null || config.ConnectionString is null || config.DatabaseName is null)
//        {
//            throw new Exception("Database is not configured");
//        }
//        return true;
//    });

builder.Services.AddOpenApi("v1", options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;

    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Contact = new OpenApiContact
        {
            Name = "Felipe Augusto, MVP",
            Url = new Uri("https://www.youtube.com/@D.E.P.L.O.Y"),
        };

        document.Info.License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        };

        document.ExternalDocs = new OpenApiExternalDocs
        {
            Description = "Canal DEPLOY - Temporal Tables",
            Url = new Uri("https://www.youtube.com/@D.E.P.L.O.Y")
        };
        return Task.CompletedTask;
    });
});

builder.Services.AddOpenApi("v2", options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;

    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Contact = new OpenApiContact
        {
            Name = "Felipe Augusto, MVP",
            Url = new Uri("https://www.youtube.com/@D.E.P.L.O.Y"),
        };
        document.Info.License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        };
        return Task.CompletedTask;
    });
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Canal DEPLOY - Temporal Tables - V1", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Canal DEPLOY - Temporal Tables - V2", Version = "v2" });
});

builder.Services.AddDbContext<DeployDbContext>(options =>
    {
        options
        .UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:DEPLOYConnection").Value!,
        p => p.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(3),
            errorNumbersToAdd: null)
        .MigrationsHistoryTable("_ControleMigracoes", "dbo"))
        .EnableSensitiveDataLogging() // habilita os parametros das instrucoes sql
        .LogTo(Console.WriteLine, LogLevel.Debug);
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    //scalar
    app.MapScalarApiReference(options =>
    { 
        options.WithTitle("Canal DEPLOY - Temporal Tables");
        options.WithTheme(ScalarTheme.BluePlanet);
        options.WithSidebar(true);
    });

    //swagger
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "swagger";
        options.SwaggerEndpoint("/openapi/v1.json", "Canal DEPLOY - Temporal Tables - V1");
        options.SwaggerEndpoint("/openapi/v2.json", "Canal DEPLOY - Temporal Tables - V2");
    });

    //redoc
    app.UseReDoc(options =>
    {
        options.DocumentTitle = "REDOC API Documentation";
        options.SpecUrl("/openapi/v1.json");
        options.RoutePrefix = "redocv1";
    });

    app.UseReDoc(options =>
    {
        options.DocumentTitle = "REDOC API Documentation";
        options.SpecUrl("/openapi/v2.json");
        options.RoutePrefix = "redocv2";
    });
}
else
{
    app.UseHttpsRedirection();
}

//Endpoint
app.MapPessoaEndpointsV1();
app.MapPessoaEndpointsV2();
//app.MapMarinasEndpoints();

await app.RunAsync();