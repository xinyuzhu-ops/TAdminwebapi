using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.ApiRes;
using Model.Dto.User;
using Service;
using xzhuWebApi.Config;

namespace xzhuWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IUserService _userService;
        private ICustomJwtService _customJWTService;
        public LoginController(IUserService userService, ICustomJwtService customJWTService)
        {
            _userService = userService;
            _customJWTService = customJWTService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetToken(string name, string password)
        {
            var res = Task.Run(() =>
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
                {
                    return ResultHelper.Error("参数不能为空");
                }
                UserRes user = _userService.GetUser(name, password);
                if (string.IsNullOrEmpty(user.Name))
                {
                    return ResultHelper.Error("账号不存在，用户名或密码错误！");
                }
                return ResultHelper.Success(_customJWTService.GetToken(user));
            });
            return await res;
        }
    }
}
