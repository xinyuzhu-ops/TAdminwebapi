using Model.ApiRes;
using Model.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IUserService
    {
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserRes GetUser(string username, string password);

        /// <summary>
        /// 用户添加
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool Add(UserAdd user, long userId);
        /// <summary>
        /// 用户编辑
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool Edit(UserEdit user, long userId);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool Del(long Id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool BatchDel(string ids);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        PageInfo GetUsers(UserReq req);

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserRes GetUserById(long id);

        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="rids"></param>
        /// <returns></returns>
        bool SettingRole(string pid, string rids);

        /// <summary>
        /// 个人中心修改昵称和密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="nickName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool EditNickNameOrPassword(long userId, string nickName, string password);
    }
}
