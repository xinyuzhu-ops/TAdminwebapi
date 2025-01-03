﻿using AutoMapper;
using Interface;
using Model.ApiRes;
using Model.Dto.User;
using Model.Entitys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService:IUserService
    {
        private readonly IMapper _mapper;
        private ISqlSugarClient _db { get; set; }
        public UserService(IMapper mapper, ISqlSugarClient db)
        {
            _mapper = mapper;
            _db = db;
        }

        //登录
        public UserRes GetUser(string username, string password)
        {
            var user = _db.Queryable<Users>().Where(u => u.Name == username && u.Password == password).First();
            if (user != null)
            {
                return _mapper.Map<UserRes>(user);
            }
            return new UserRes();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool Add(UserAdd user, long userId)
        {
            var info = _mapper.Map<Users>(user);
            info.CreateUserId = userId;
            info.CreateDate = DateTime.Now;
            info.IsDeleted = 0;
            info.UserType = 1;
            return _db.Insertable(info).ExecuteCommand() > 0; ;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool Edit(UserEdit user, long userId)
        {
            var info = _db.Queryable<Users>().First(p => p.Id == user.Id);
            _mapper.Map(user, info);
            info.ModifyDate = DateTime.Now;
            info.ModifyUserId = userId;
            return _db.Updateable(info).ExecuteCommand() > 0;

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Del(long Id)
        {
            var info = _db.Queryable<Users>().First(p => p.Id == Id);
            return _db.Deleteable(info).ExecuteCommand() > 0;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool BatchDel(string ids)
        {
            return _db.Ado.ExecuteCommand($"DELETE Users WHERE Id IN({ids})") > 0;
        }

        public PageInfo GetUsers(UserReq req)
        {
            PageInfo pageInfo = new PageInfo();
            var exp = _db.Queryable<Users>()
                //默认只查询非炒鸡管理员的用户
                .Where(u => u.UserType == 1)
                .WhereIF(!string.IsNullOrEmpty(req.Name), u => u.Name.Contains(req.Name))
                .WhereIF(!string.IsNullOrEmpty(req.NickName), u => u.NickName.Contains(req.NickName))
                .OrderBy((u) => u.CreateDate, OrderByType.Desc)
                .Select((u) => new UserRes
                {
                    Id = u.Id,
                    Name = u.Name,
                    NickName = u.NickName,
                    Password = u.Password,
                    UserType = u.UserType,
                    CreateDate = u.CreateDate,
                    IsEnable = u.IsEnable,
                    Description = u.Description
                });
            var res = exp
                .Skip((req.PageIndex - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToList();
            res.ForEach(p =>
            {
                p.RoleName = GetRolesByUserId(p.Id);
            });
            pageInfo.Data = _mapper.Map<List<UserRes>>(res);
            pageInfo.Total = exp.Count();
            return pageInfo;
        }
        private string GetRolesByUserId(long uid)
        {
            return _db.Ado.SqlQuery<string>($@"SELECT STUFF((SELECT ','+R.Name FROM dbo.Role R
                    LEFT JOIN dbo.UserRoleRelation UR ON R.Id=UR.RoleId
                    WHERE UR.UserId={uid} FOR XML PATH('')),1,1,'') RoleNames")[0];
        }

        public UserRes GetUserById(long id)
        {
            var info = _db.Queryable<Users>().First(p => p.Id == id);
            return _mapper.Map<UserRes>(info);
        }

        //设置角色
        public bool SettingRole(string pid, string rids)
        {
            List<UserRoleRelation> list = new List<UserRoleRelation>();
            foreach (string it in rids.Split(','))
            {
                UserRoleRelation info = new UserRoleRelation() { UserId = Convert.ToInt64(pid), RoleId = Convert.ToInt64(it.Replace("'", "")) };
                list.Add(info);
            }
            //删除之前的角色
            _db.Ado.ExecuteCommand($"DELETE UserRoleRelation WHERE UserId = {pid}");
            return _db.Insertable(list).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 个人中心修改昵称和密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="nickName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool EditNickNameOrPassword(long userId, string nickName, string password)
        {
            var info = _db.Queryable<Users>().Where(p => p.Id == userId).First();
            if (info != null)
            {
                if (!string.IsNullOrEmpty(nickName))
                {
                    info.NickName = nickName;
                }
                if (!string.IsNullOrEmpty(password))
                {
                    info.Password = password;
                }
            }
            return _db.Updateable(info).ExecuteCommand() > 0;
        }
    }
}
