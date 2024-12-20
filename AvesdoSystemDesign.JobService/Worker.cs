using AvesdoSystemDesign.Domain.Entities;
using AvesdoSystemDesign.Infrastructure.Services;
using AvesdoSystemDesign.JobService.ExternalServices;

namespace AvesdoSystemDesign.JobService;

public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            try
            {
                await using var scope = serviceProvider.CreateAsyncScope();
                
                var externalService = scope.ServiceProvider.GetRequiredService<IExService>();
                var leadService = scope.ServiceProvider.GetRequiredService<ILeadService>();
                var leadsFromExternalService = externalService.GetLeads();
                var leadEntities = leadsFromExternalService.Select(l => new Lead
                {
                    Email = l.Email,
                    FirstName = l.FirstName,
                    LastName = l.LastName,

                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList();

                await leadService.AddLeadsAsync(leadEntities);

                logger.LogInformation("Added {Count} leads to the database.", leadEntities.Count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing leads.");
                // More sophisticated error handling (retries, circuit breaker)
            }
            
            
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}