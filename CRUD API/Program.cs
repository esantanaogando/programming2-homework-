using CRUD_API.Models;
using Microsoft.EntityFrameworkCore;
using Pets.Application.Contract;
using Pets.Application.Services;
using Pets.Infrastructure.Repositories;
using Pets.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PetDataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<PetRepository>();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<IPetService, PetService>();



builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();

}, typeof(Program).Assembly);



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
