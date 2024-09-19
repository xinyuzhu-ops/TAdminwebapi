using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.ApiRes;
using Model.Dto.Role;
using xzhuWebApi.Config;

namespace xzhuWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _role;

        public RoleController(IRoleService role)
        {
            _role = role;
        }
        //获取角色
        [HttpPost]
        public ApiResult GetRoles(RoleReq req)
        {
            return ResultHelper.Success(_role.GetRoles(req));
        }
        //根据id获取角色
        [HttpGet]
        public ApiResult GetRole(long id)
        {
            return ResultHelper.Success(_role.GetRoleById(id));
        }
        //新增角色
        [HttpPost]
        public ApiResult Add(RoleAdd req)
        {
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            return ResultHelper.Success(_role.Add(req, userId));
        }
        //编辑
        [HttpPost]
        public ApiResult Edit(RoleEdit req)
        {
            //获取当前登录人信息
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            return ResultHelper.Success(_role.Edit(req, userId));
        }
        //删除
        [HttpGet]
        public ApiResult Del(long id)
        {
            //获取当前登录人信息 
            return ResultHelper.Success(_role.Del(id));
        }
        //批量删除
        [HttpGet]
        public ApiResult BatchDel(string ids)
        {
            //获取当前登录人信息 
            return ResultHelper.Success(_role.BatchDel(ids));
        }
    }
}
