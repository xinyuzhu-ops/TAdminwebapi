using AutoMapper;
using Model.Dto.Menu;
using Model.Dto.Role;
using Model.Dto.User;
using Model.Entitys;

namespace xzhuWebApi.Config
{
    /// <summary>
    /// DTO映射配置
    /// </summary>
    public class AutoMapperConfigs:Profile
    {
        public AutoMapperConfigs()
        {
            //角色
            CreateMap<Role, RoleRes>();
            CreateMap<RoleAdd, Role>();
            CreateMap<RoleEdit, Role>();
            //用户
            CreateMap<Users, UserRes>();
            CreateMap<UserAdd, Users>();
            CreateMap<UserEdit, Users>();
            //菜单
            CreateMap<Menu, MenuRes>();
            CreateMap<MenuAdd, Menu>();
            CreateMap<MenuEdit, Menu>();
        }


    }
}
