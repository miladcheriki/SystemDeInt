using AvesdoSystemDesign.Domain.Entities;
using AvesdoSystemDesign.Shared;
using AvesdoSystemDesign.Shared.Models;

namespace AvesdoSystemDesign.Infrastructure.Services;

public interface ILeadService
{
    Task AddLeadsAsync(List<Lead> leads);
    Task<LeadDto?> GetLeadAsync(int leadId);
    Task<List<LeadDto>> GetLeadsAsync(string? searchTerm, SortEnum? sort);
}