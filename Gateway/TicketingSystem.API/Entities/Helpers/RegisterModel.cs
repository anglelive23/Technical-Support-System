namespace Gateway.API.Entities.Helpers
{
    public class RegisterModel
    {
        [Required, MaxLength(20)]
        public string FirstName { get; set; }
        [Required, MaxLength(20)]
        public string LastName { get; set; }
        [Required, MaxLength(50)]
        public string UserName { get; set; }
        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(100)]
        public string Password { get; set; }
        [Required, MaxLength(20)]
        public string Role { get; set; }
    }
}
