
using Microsoft.OpenApi.Models;
using TeacherPortal.Application;
using TeacherPortal.Infrastructure;
using TeacherPortal.WebAPI;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("http://localhost:3001") // Matching with our teacher portal ui app
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TeacherPortal API",
        Version = "v1",
        Description = "API documentation for TeacherPortal"
    });
});



builder.Services.AddControllers();


var app = builder.Build();

app.UseCors("AllowLocalhost");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TeacherPortal API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<TeacherPortal.Application.Common.Exceptions.GlobalExceptionHandler>();
app.UseMiddleware<TeacherPortal.Application.Common.Middleware.AuthenticationMiddleware>();
MigrationRunner.RunMigrations(builder.Configuration);

app.Run();
