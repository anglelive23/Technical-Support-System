namespace TicketService.Entities.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Content { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        [Required]
        public string UserId { get; set; }
        public Ticket? Ticket { get; set; }
        [Required]
        public int TicketId { get; set; }
    }
}
