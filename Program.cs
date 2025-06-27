using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tienda.Microservicios.Autor.Api.Extensions;
using Tienda.Microservicios.Autor.Api.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores y servicios personalizados
builder.Services.AddControllers();
builder.Services.AddCustomServices(builder.Configuration);

// Configurar CORS para permitir acceso desde React en puerto 3012
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3012") // Cambia si tu frontend cambia de URL o puerto
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Microservicio de Autores API",
        Version = "v1",
        Description = "API para gestionar autores",
        Contact = new OpenApiContact
        {
            Name = "Equipo de Desarrollo",
            Email = "desarrollo@tienda.com"
        }
    });
});

// DbContext con SQL Server
builder.Services.AddDbContext<ContextoAutor>(options =>
    options.UseSqlServer(builder.Configuration["DefaultConnection"]));


// Configuración Kestrel para escuchar en puerto 8081
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // ✅ Obligatorio en Render
});


var app = builder.Build();

// IMPORTANTE: Usa CORS ANTES de MapControllers y Swagger
app.UseCors("PermitirFrontend");

// No usar redirección a HTTPS si no tienes certificado válido
// app.UseHttpsRedirection();

app.UseAuthorization();

// Swagger middleware
app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Autor API V1");
    c.RoutePrefix = "swagger";
    c.ConfigObject.AdditionalItems["persistAuthorization"] = false;
    c.ConfigObject.AdditionalItems["tryItOutEnabled"] = true;
    // Puedes eliminar HeadContent si no lo usas
});

// Mapear endpoints de controladores
app.MapControllers();


app.Run();
