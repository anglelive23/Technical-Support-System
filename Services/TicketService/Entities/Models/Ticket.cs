namespace TicketService.Entities.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Description { get; set; }
        // --> Users
        public ApplicationUser? Client { get; set; }
        public string? ClientId { get; set; }
        public ApplicationUser? Employee { get; set; }
        public string? EmployeeId { get; set; }
        // -->
        // --> Priority
        public Priority? Priority { get; set; }
        public int? PriorityId { get; set; } = 1;
        // -->
        // --> Department
        public Department? Department { get; set; }
        public int? DepartmentId { get; set; }
        // -->
        // --> Project
        public Project? Project { get; set; }
        public int ProjectId { get; set; }
        // -->
        // --> Status
        public Status? Status { get; set; }
        public int StatusId { get; set; }
        // -->
        // --> Company
        public Company? Company { get; set; }
        public int? CompanyId { get; set; }
        // -->
        // --> Comments
        public IList<Comment>? Comments { get; set; }
        // -->
        [MaxLength(11)]
        public string? PhoneNumber
        {
            get
            {
                return Client?.PhoneNumber;
            }
            set { }
        }
        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email
        {
            get { return Client?.Email; }
            set { }
        }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? LastSeen { get; set; }
        public DateTime? EstimatedTime { get; set; }
        public string? Reporter { get; set; }
    }
}
