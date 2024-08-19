
using Microsoft.EntityFrameworkCore;
using WebLibrary.Context;
using WebLibrary.Services;
using Microsoft.OpenApi.Models;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebLibrary API", Version = "v1" });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

builder.Services.AddScoped<AutorService>();
builder.Services.AddScoped<LibroService>();

var app = builder.Build();


app.UseMiddleware<BasicAuthMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebLibrary API v1");
        c.RoutePrefix = "swagger"; 
    });
}

app.UseStaticFiles(); 

app.UseRouting();
app.UseAuthorization();
app.MapGet("/", () => Results.Redirect("/index.html")); 
app.MapControllers();

app.Run();

