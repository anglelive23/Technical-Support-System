namespace Gateway.API.Entities.Dtos.Response
{
    public class ApplicationUserResponseDto
    {
        public string? Id { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        [Required, MaxLength(20)]
        public string FirstName { get; set; } = string.Empty;
        [JsonIgnore]
        [IgnoreDataMember]
        [Required, MaxLength(20)]
        public string LastName { get; set; } = string.Empty;

        public string Name
        {
            get
            {
                return $"{FirstName + ' ' + LastName}";
            }
            set { }
        }
    }
}
