using AvesdoSystemDesign.Shared.Models;

namespace AvesdoSystemDesign.JobService.ExternalServices;

public class ExService : IExService
{
    public List<LeadDto> GetLeads()
        => FakeLeadGenerator.GenerateLeads(10);
}