using AvesdoSystemDesign.Shared.Models;
using Bogus;

namespace AvesdoSystemDesign.JobService.ExternalServices;

public static class FakeLeadGenerator
{
    public static List<LeadDto> GenerateLeads(int count)
    {
        var leadFaker = new Faker<LeadDto>()
            .RuleFor(l => l.FirstName, f => f.Name.FirstName())
            .RuleFor(l => l.LastName, f => f.Name.LastName())
            .RuleFor(l => 
                l.Email, (f, l) => f.Internet.Email(l.FirstName, l.LastName));

        return leadFaker.Generate(count);
    }
}