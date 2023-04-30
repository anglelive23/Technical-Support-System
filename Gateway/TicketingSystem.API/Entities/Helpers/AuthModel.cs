using System.Text.Json.Serialization;

namespace Gateway.API.Entities.Helpers
{
    public class AuthModel
    {
        public string Message { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; } = false;
        public string UserName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IList<string> Roles { get; set; }
        public string Token { get; set; } = string.Empty;
        //[JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
