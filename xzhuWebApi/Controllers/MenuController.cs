using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.ApiRes;
using Model.Dto.Menu;
using xzhuWebApi.Config;

namespace xzhuWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        //获取菜单
        [HttpPost]
        public ApiResult GetMenus(MenuReq req)
        {
            return ResultHelper.Success(_menuService.GetMenus(req));
        }

        //新增
        [HttpPost]
        public ApiResult Add(MenuAdd req)
        {
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            //获取登录人当前信息
            return ResultHelper.Success(_menuService.Add(req, userId));
        }
        //编辑
        [HttpPost]
        public ApiResult Edit(MenuEdit req)
        {
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            return ResultHelper.Success(_menuService.Edit(req, userId));
        }
        //删除
        [HttpGet]
        public ApiResult Del(long id)
        {
            return ResultHelper.Success(_menuService.Del(id));
        }
        //批量删除
        [HttpGet]
        public ApiResult BatchDel(string ids)
        {
            return ResultHelper.Success(_menuService.BatchDel(ids));

        }


        //设置菜单
        [HttpGet]
        public ApiResult SettingMenu(long rid, string mids)
        {
            return ResultHelper.Success(_menuService.SettingMenu(rid, mids));
        }
        //获取当前登录用户拥有的菜单
        [HttpGet]
        public ApiResult GetUserMenus()
        {
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            return ResultHelper.Success(_menuService.GetMenusByUserId(userId));
        }




    }
}
