using System;
using System.Reflection;
using AuctionService.Data.Seeding;
using FluentValidation;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartCredit.BackEnd.Application.Features.CreditCards.Queries;
using SmartCredit.BackEnd.Application.Mapping;
using SmartCredit.BackEnd.Domain.Contracts;
using SmartCredit.BackEnd.Domain.Interfaces;
using SmartCredit.BackEnd.Persistence.Context;
using SmartCredit.BackEnd.Persistence.Services;
using SmartCredit.BackEnd.WebApi.Healthcheck;
using SmartCredit.BackEnd.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(opc => {
    opc.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("MyPolicy", builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(origin => true);
}));

builder.Services.AddAutoMapper(typeof(AutomapperProfile));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCreditCardsList).Assembly));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");
app.UseMiddleware<ExceptionMiddleware>();
//app.UseHttpsRedirection();
app.MapControllers();

//HealthCheck Middleware
app.MapHealthChecks("/api/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseHealthChecksUI(delegate (Options options)
{
    options.UIPath = "/healthcheck-ui";
});

DbInitializer.InitDb(app);

app.Run();