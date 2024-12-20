using AvesdoSystemDesign.Domain.Entities;
using AvesdoSystemDesign.Infrastructure.Cache;
using AvesdoSystemDesign.Infrastructure.Db;
using AvesdoSystemDesign.Shared;
using AvesdoSystemDesign.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AvesdoSystemDesign.Infrastructure.Services;

public class LeadService(LeadsDbContext context, ICacheService cacheService) : ILeadService
{
    public async Task AddLeadsAsync(List<Lead> leads)
    {
        context.Leads.AddRange(leads);
        await context.SaveChangesAsync();
    }

    public async Task<LeadDto?> GetLeadAsync(int leadId)
    {
        var cacheKey = $"lead_{leadId}_cache_key";
        var leadDto = await cacheService.GetAsync<LeadDto>(cacheKey);

        if (leadDto != null)
            return leadDto;
        
        var lead = await context.Leads.FindAsync(leadId);
        
        if (lead == null)
            return null;
        
        leadDto = new LeadDto
        {
            Id = lead.Id,
            Email = lead.Email,
            FirstName = lead.FirstName,
            LastName = lead.LastName,
            CreatedAt = lead.CreatedAt
        };

        await cacheService.SetAsync(cacheKey, leadDto, TimeSpan.FromHours(1));
        
        return leadDto;
    }
    
    public async Task<List<LeadDto>> GetLeadsAsync(string? searchTerm, SortEnum? sort)
    {
        var cacheKey = $"GetLeads_{searchTerm}_{sort}";
        var cachedLeads = await cacheService.GetAsync<List<LeadDto>>(cacheKey);
        
        if (cachedLeads != null) 
            return cachedLeads;
        
        var query = context.Leads.AsNoTracking();
        
        searchTerm = searchTerm?.Trim();
        if (searchTerm != null)
        {
            if (Helpers.IsValidEmail(searchTerm))
            {
                query = query.Where(l => l.Email == searchTerm);
            }
            else
            {
                query = query.Where(l =>
                    EF.Functions.Like(l.Email, $"{searchTerm}%") ||
                    EF.Functions.Like(l.FirstName, $"{searchTerm}%") ||
                    EF.Functions.Like(l.LastName, $"{searchTerm}%"));
            }
        }
        
        if (sort.HasValue)
        {
            query = sort.Value switch
            {
                SortEnum.ASC => query.OrderBy(l => l.Email),
                SortEnum.DESC => query.OrderByDescending(l => l.Email)
            };
        }
        else 
            query = query.OrderBy(l => l.Id);

        var leads = await query
            .Select(l => new LeadDto
            {
                Id = l.Id,
                Email = l.Email,
                FirstName = l.FirstName,
                LastName = l.LastName,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync();
        
        if(leads.Count != 0)
            await cacheService.SetAsync(cacheKey, leads, TimeSpan.FromMinutes(10));
        
        return leads;
    }
}