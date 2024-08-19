//using Microsoft.EntityFrameworkCore;
//using WebLibrary.Context;
//using WebLibrary.Services;
//using Microsoft.OpenApi.Models;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebLibrary API", Version = "v1" });
//});

//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

//// Tengo que registrar los servicios
//builder.Services.AddScoped<AutorService>();
//builder.Services.AddScoped<LibroService>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseAuthorization();

//app.MapControllers();

//app.Run();


using Microsoft.EntityFrameworkCore;
using WebLibrary.Context;
using WebLibrary.Services;
using Microsoft.OpenApi.Models;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebLibrary API", Version = "v1" });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

// Register services
builder.Services.AddScoped<AutorService>();
builder.Services.AddScoped<LibroService>();

var app = builder.Build();

// Register the BasicAuthMiddleware to protect Swagger
app.UseMiddleware<BasicAuthMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebLibrary API v1");
        c.RoutePrefix = "swagger"; // Optional: change Swagger URL prefix
    });
}

app.UseStaticFiles(); // Servir archivos estáticos

app.UseRouting();
app.UseAuthorization();
app.MapGet("/", () => Results.Redirect("/index.html")); // Redirigir a index.html
app.MapControllers();

app.Run();
