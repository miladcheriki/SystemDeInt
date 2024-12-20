using AvesdoSystemDesign.Infrastructure.Db;
using AvesdoSystemDesign.Infrastructure.Services;
using AvesdoSystemDesign.JobService;
using AvesdoSystemDesign.JobService.ExternalServices;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddTransient<IExService, ExService>();
builder.Services.AddTransient<ILeadService, LeadService>();

builder.Services.AddDbContext<LeadsDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("ReadWrite")));

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LeadsDbContext>();
    dbContext.Database.EnsureCreated();
}

host.Run();