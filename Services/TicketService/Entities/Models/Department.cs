namespace TicketService.Entities.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(15)]
        public string Name { get; set; } = string.Empty;
    }
}
