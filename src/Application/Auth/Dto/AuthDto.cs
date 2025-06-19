namespace Application.Auth.Dto
{
    public class AuthDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required List<string> Permissions { get; set; }
    }
}
