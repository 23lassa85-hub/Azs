using AzsApi.Data;
using AzsApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AzsContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Сервисы: вся работа с базой данных находится в них
builder.Services.AddScoped<IFirmyService, FirmyService>();
builder.Services.AddScoped<IAzsService, AzsService>();
builder.Services.AddScoped<IToplivoService, ToplivoService>();
builder.Services.AddScoped<IFirmyToplivoService, FirmyToplivoService>();
builder.Services.AddScoped<IKlientyService, KlientyService>();
builder.Services.AddScoped<IProdazhiService, ProdazhiService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
