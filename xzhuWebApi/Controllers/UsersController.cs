using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.ApiRes;
using Model.Dto.User;
using xzhuWebApi.Config;

namespace xzhuWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        //获取用户列表
        public ApiResult GetUsers(UserReq req)
        {
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            return ResultHelper.Success(_userService.GetUsers(req));
        }

        //根据用户Id获取用户
        [HttpGet]
        public ApiResult GetUsersById(long id)
        {
            return ResultHelper.Success(_userService.GetUserById(id));
        }

        //添加
        [HttpPost]
        public ApiResult Add(UserAdd req)
        {
            //获取当前登录人信息 
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            return ResultHelper.Success(_userService.Add(req, userId));
        }
        //编辑
        [HttpPost]
        public ApiResult Edit(UserEdit req)
        {
            //获取当前登录人信息
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            return ResultHelper.Success(_userService.Edit(req, userId));
        }
        //删除
        [HttpGet]
        public ApiResult Del(long id)
        {
            return ResultHelper.Success(_userService.Del(id));
        }
        //批量删除
        [HttpGet]
        public ApiResult BatchDel(string ids)
        {
            return ResultHelper.Success(_userService.BatchDel(ids));
        }

        [HttpGet]
        public ApiResult SettingRole(string pid, string rids)
        {
            return ResultHelper.Success(_userService.SettingRole(pid, rids));
        }

        [HttpGet]
        public ApiResult EditNickNameOrPassword(string nickName, string password)
        {
            //获取当前登录人信息
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            return ResultHelper.Success(_userService.EditNickNameOrPassword(userId, nickName, password));
        }
    }
}
