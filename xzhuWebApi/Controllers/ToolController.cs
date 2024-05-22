using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using SqlSugar;
using System.Reflection;

namespace xzhuWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToolController : ControllerBase
    {
        public ISqlSugarClient _db;
        public ToolController(ISqlSugarClient db) 
        {
        
            _db = db;
        }

        [HttpGet]
        public string InitDateBase()
        {
            string res = "ok";
            //创建库
            _db.DbMaintenance.CreateDatabase();
            //创建表
            string nspace = "Model.Entitys";
            Type[] ass = Assembly.LoadFrom(AppContext.BaseDirectory + "Model.dll").GetTypes().Where(p => p.Namespace == nspace).ToArray();
            _db.CodeFirst.SetStringDefaultLength(200).InitTables(ass);
            //初始化数据
            Users user = new Users()
            {
                Name = "admin",
                NickName = "管理员",
                Password = "123456",
                UserType = 0,
                IsEnable = true,
                Description = "数据库初始化添加管理员",
                CreateDate = DateTime.Now,
                CreateUserId = 0,
                IsDeleted = 0
            };
            long userId = _db.Insertable(user).ExecuteReturnBigIdentity();

            Menu menuRoot = new Menu()
            {
                Name = "菜单管理",
                Index = "menumanager",
                FilePath = "../views/admin/menu/MenuManager",
                ParentId = 0,
                Order = 0,
                IsEnable = true,
                Description = "数据库初始化添加的默认菜单",
                CreateDate = DateTime.Now,
                CreateUserId = userId,
                IsDeleted = 0
            };
            _db.Insertable(menuRoot).ExecuteReturnBigIdentity();
            return res;
        }
    }

}
