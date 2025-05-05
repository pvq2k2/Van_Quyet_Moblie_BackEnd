namespace DTOs.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? NumberPhone { get; set; }
        public string? Avatar { get; set; }
        public int Gender { get; set; }
        public int Status { get; set; }
        public List<RoleUserInUserDTO>? ListRoleUser { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class RoleUserInUserDTO
    {
        public int Id { get; set; }
        public RoleInUserDTO? Role { get; set; }
    }

    public class RoleInUserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
    }
}
