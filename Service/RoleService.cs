using AutoMapper;
using Interface;
using Model.ApiRes;
using Model.Dto.Role;
using Model.Entitys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RoleService: IRoleService
    {
        private readonly IMapper _mapper;
        private ISqlSugarClient _db { get; set; }
        public RoleService(IMapper mapper, ISqlSugarClient db)
        {
            _mapper = mapper;
            _db = db;
        }
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool Add(RoleAdd req, long userId)
        {
            Role info = _mapper.Map<Role>(req);
            info.CreateUserId = userId;
            info.CreateDate = DateTime.Now;
            info.IsDeleted = 0;
            return _db.Insertable(info).ExecuteCommand() > 0;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool BatchDel(string ids)
        {
            return _db.Ado.ExecuteCommand($"DELETE Role WHERE Id IN({ids})") > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Del(long id)
        {
            var info = _db.Queryable<Role>().First(p => p.Id == id);
            return _db.Deleteable(info).ExecuteCommand() > 0;
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Edit(RoleEdit req, long userId)
        {
            var role = _db.Queryable<Role>().First(p => p.Id == req.Id);
            _mapper.Map(req, role);
            role.ModifyDate = DateTime.Now;
            role.ModifyUserId = userId;
            return _db.Updateable(role).ExecuteCommand() > 0;
        }
        /// <summary>
        /// 根据id获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RoleRes GetRoleById(long id)
        {
            var info = _db.Queryable<Role>().First(p => p.Id == id);
            return _mapper.Map<RoleRes>(info);
        }
        /// <summary>
        /// 获取全部角色分页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public PageInfo GetRoles(RoleReq req)
        {
            PageInfo pageInfo = new PageInfo();
            var exp = _db.Queryable<Role>().WhereIF(!string.IsNullOrEmpty(req.Name), p => p.Name.Contains(req.Name));
            var res = exp
                .OrderBy(p => p.Order)
                .Skip((req.PageIndex - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToList();
            pageInfo.Data = _mapper.Map<List<RoleRes>>(res);
            pageInfo.Total = exp.Count();
            return pageInfo;
        }




    }
}
