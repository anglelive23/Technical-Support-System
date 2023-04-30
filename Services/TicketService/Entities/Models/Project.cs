namespace TicketService.Entities.Models
{
    public class Project
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
        public Company? Company { get; set; }
        public int? CompanyId { get; set; }
        public ApplicationUser? Client { get; set; }
        public string? ClientId { get; set; }
    }
}
