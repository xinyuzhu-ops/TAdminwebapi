using AutoMapper;
using Interface;
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
    }
}
