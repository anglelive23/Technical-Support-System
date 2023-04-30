namespace Gateway.API.Entities.Helpers
{
    public class LoginModel
    {
        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(50)]
        public string Password { get; set; }
    }
}
