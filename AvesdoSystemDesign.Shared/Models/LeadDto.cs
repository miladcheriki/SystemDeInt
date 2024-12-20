namespace AvesdoSystemDesign.Shared.Models;

public class LeadDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    public DateTime CreatedAt { get; set; }
}