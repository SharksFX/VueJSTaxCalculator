using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using webapi.Models.ContextEF;


var builder = WebApplication.CreateBuilder(args);
string _policyName = "CorsPolicy";

// Add services to the container.
var config = builder.Configuration.AddJsonFile("appsettings.json",
        optional: true,
        reloadOnChange: true).Build();

builder.Services.AddDbContext<TaxablesDBContext>(opt =>
    opt.UseSqlServer(config.GetConnectionString("DbConn")));

    builder.Services.AddCors(opt =>
    {
        opt.AddPolicy(name: _policyName, builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(_policyName);
app.UseAuthorization();

app.MapControllers();

DataSeeder.Seed(app.Services);

app.Run();
