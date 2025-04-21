using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using WebApiAfternoon.Data;
using WebApiAfternoon.Formatters;
using WebApiAfternoon.Middlewares;
using WebApiAfternoon.Repositories.Abstract;
using WebApiAfternoon.Repositories.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();


var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<StudentDbContext>(opt =>
{
    opt.UseSqlServer(conn);
});
builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
});


var app = builder.Build();
app.UseStaticFiles();
// Configure the HTTP request pipeline.
app.UseMiddleware<GlobalErrorHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomAuthMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();
