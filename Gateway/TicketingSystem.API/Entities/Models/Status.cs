namespace Gateway.API.Entities.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; } = string.Empty;
    }
}
