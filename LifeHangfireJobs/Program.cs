using Hangfire;
using lifeEcommerce.Data;
using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Helpers;
using LifeHangfireJobs.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHangfire(x => 
                x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddDbContext<LifeEcommerceDbContext>(options =>
                                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var smtpConfiguration = builder.Configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();
builder.Services.AddSingleton(smtpConfiguration);
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


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

app.UseHangfireDashboard();

app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();

var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
var emailSender = scope.ServiceProvider.GetService<IEmailSender>();

LifeService lifeService = new(unitOfWork, emailSender);

//RecurringJob.AddOrUpdate("NotifyAdmin", () => lifeService.NotifyAdmin(), Cron.Minutely);
RecurringJob.AddOrUpdate("GenerateMetrics", () => lifeService.GetMetrics(), Cron.Hourly);

app.Run();
