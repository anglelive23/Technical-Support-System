namespace Gateway.API.Entities.Dtos.Request
{
    public class TicketRequestDto
    {
        public int ProjectId { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Description { get; set; }
        public string? ClientId { get; set; }
        public int StatusId { get; set; } = 1;
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
