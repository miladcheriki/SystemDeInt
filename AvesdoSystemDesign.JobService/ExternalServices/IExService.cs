using AvesdoSystemDesign.Shared.Models;

namespace AvesdoSystemDesign.JobService.ExternalServices;

public interface IExService
{
    List<LeadDto> GetLeads();
}