using Autofac;
using Autofac.Extensions.DependencyInjection;
using Model.ApiRes;
using SqlSugar;

namespace xzhuWebApi.Config
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class ExtendHostBuilder
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="app"></param>
        public static void Register(this WebApplicationBuilder app)
        {

            //所有的注册全部写在这里---外层调用Register就可以了
            app.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //替换工厂类使用autofac
            app.Host.ConfigureContainer<ContainerBuilder>(builder => 
            {
                #region 注册SQL sugar
                builder.Register<ISqlSugarClient>(context => 
                {
                    SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
                    {
                        //连接字符串
                        ConnectionString = "Data Source=SQLSERVER2019;Initial Catalog=TSAdminDb;Persist Security Info=True;User ID=sa;Password=xx",
                        DbType = DbType.SqlServer,  //数据库类型 sqlserver/  myssql等很多具体去SQL sugar官网上查
                        IsAutoCloseConnection = true,  //是否自动关闭连接
                    });
                    //sql语句的输出，方便排查问题
                    db.Aop.OnLogExecuted = (sql, par) =>
                    {
                        Console.WriteLine("\r\n");
                        Console.WriteLine($"{DateTime.Now.ToString("yyyyMMdd HH:mm:ss")}，Sql语句：{sql}");
                        Console.WriteLine("===========================================================================");
                    };
                    return db;
                });
                #endregion

                //注册接口和实现层
                builder.RegisterModule(new AutofacRegister());
                
            });

            //Automapper映射
            app.Services.AddAutoMapper(typeof(AutoMapperConfigs));
            //第一步，注册JWT
            app.Services.Configure<JWTToken>(app.Configuration.GetSection("JWTToken"));
        }
    }
}
