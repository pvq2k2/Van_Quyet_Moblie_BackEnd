namespace DTOs.Role
{
    public class GetAllDataRoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public required List<PermissionRoleInGetAllDataRoleDTO> ListPermissionRole { get; set; }
        public required List<RoleMenuInGetAllDataRoleDTO> ListRoleMenu { get; set; }
    }

    public class PermissionRoleInGetAllDataRoleDTO
    {
        public int Id { get; set; }

        public PermissionInGetAllDataRoleDTO? Permission { get; set; }
    }

    public class RoleMenuInGetAllDataRoleDTO
    {
        public int Id { get; set; }

        public MenuInGetAllDataRoleDTO? Menu { get; set; }
    }

    public class MenuInGetAllDataRoleDTO
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

    }
    public class PermissionInGetAllDataRoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
