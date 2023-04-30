namespace TicketService.Entities.Models
{
    public class Priority
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(15)]
        public string Name { get; set; } = string.Empty;
    }
}
