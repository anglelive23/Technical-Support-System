namespace Gateway.API.Entities.Dtos.Response
{
    public class MinifiedTicketResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ClientId { get; set; }
        public ApplicationUserResponseDto? Client { get; set; }
        public string? EmployeeId { get; set; }
        public ApplicationUserResponseDto? Employee { get; set; }
        public int? PriorityId { get; set; }
        public PriorityResponseDto? Priority { get; set; }
        public int? DepartmentId { get; set; }
        public DepartmentResponseDto? Department { get; set; }
        public int ProjectId { get; set; }
        public ProjectResponseDto? Project { get; set; }
        public int StatusId { get; set; }
        public StatusResponseDto? Status { get; set; }
        public int? CompanyId { get; set; }
        public CompanyResponseDto? Company { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? LastSeen { get; set; }
        public DateTime? EstimatedTime { get; set; }
        public string? Reporter { get; set; }
    }
}
