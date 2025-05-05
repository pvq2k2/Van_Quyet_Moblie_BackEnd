using AutoMapper;
using DTOs.Auth;
using DTOs.Role;
using DTOs.User;
using Models;

namespace Converters
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region User
            // User DTO
            CreateMap<Users, UserDTO>();
            CreateMap<RoleUser, RoleUserInUserDTO>();
            CreateMap<Roles, RoleInUserDTO>();

            // GetAllDataUserDTO
            #endregion

            #region Role
            CreateMap<Roles, RoleDTO>();
            CreateMap<Roles, RoleTokenDTO>();
            //Get All Data Role
            CreateMap<Roles, GetAllDataRoleDTO>();
            CreateMap<PermissionRole, PermissionRoleInGetAllDataRoleDTO>();
            CreateMap<Permissions, PermissionInGetAllDataRoleDTO>();
            CreateMap<RoleMenu, RoleMenuInGetAllDataRoleDTO>();
            CreateMap<Menus, MenuInGetAllDataRoleDTO>();
            #endregion

            #region Auth
            CreateMap<Users, AuthDTO>()
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());
            #endregion
        }
    }
}
