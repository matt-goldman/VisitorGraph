using GraphVisitor.WebApi.Data;
using GraphVisitor.WebApi.Models;
using GraphVisitor.WebApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

builder.Services.Configure<GraphOptions>(opt => builder.Configuration.GetSection(nameof(GraphOptions)).Bind(opt));


builder.Services.AddSingleton<IGraphService, GraphService>();
builder.Services.AddSingleton<IStaffService, StaffService>();
builder.Services.AddSingleton<IVisitService, VisitService>();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
