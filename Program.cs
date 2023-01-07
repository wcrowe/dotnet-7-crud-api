using System.Text.Json.Serialization;
using WebApi.Helpers;
using WebApi.Services;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    // ReSharper disable once UnusedVariable
    var env = builder.Environment;

    services.AddDbContext<DataContext>();
    services.AddCors();

    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params (e.g. User update)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" }); });
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // configure DI for application services
    services.AddScoped<IUserService, UserService>();
}

var app = builder.Build();

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "My API V1"); });
}

app.Run("http://localhost:4000");