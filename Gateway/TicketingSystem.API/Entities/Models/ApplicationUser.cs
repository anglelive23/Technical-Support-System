namespace Gateway.API.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(20)] public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(20)] public string LastName { get; set; } = string.Empty;
        public IList<RefreshToken>? RefreshTokens { get; set; }
    }
}
