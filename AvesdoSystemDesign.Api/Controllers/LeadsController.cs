using AvesdoSystemDesign.Infrastructure.Services;
using AvesdoSystemDesign.Shared;
using AvesdoSystemDesign.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AvesdoSystemDesign.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/leads")]
public class LeadsController(ILogger<LeadsController> logger, ILeadService leadService) : ControllerBase
{
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LeadDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeadDto>> Get(int id)
    {
        var lead = await leadService.GetLeadAsync(id);
        
        if (lead is null)
            return NotFound();
        
        return Ok(lead);
    }
    
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LeadDto>))]
    public async Task<ActionResult<List<LeadDto>>> List(string? search, SortEnum? order)
    {
        var leads = 
            await leadService.GetLeadsAsync(search, order);
        
        return Ok(leads);
    }
}