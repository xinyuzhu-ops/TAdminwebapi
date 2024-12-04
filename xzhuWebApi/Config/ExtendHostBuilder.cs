using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Model.ApiRes;
using Newtonsoft.Json;
using SqlSugar;
using System.Text;

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
                        ConnectionString = "Data Source=xxxxxxx;Initial Catalog=TSAdminDb;User ID=sa;Password=xxxxxx;Encrypt=True;TrustServerCertificate=True",
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

            //jwt验证
            #region jwt验证
            JWTToken tokenOptions = new JWTToken();
            app.Configuration.Bind("JWTToken", tokenOptions);
            app.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //JWT有一些默认的属性
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = tokenOptions.Audience,//
                        ValidIssuer = tokenOptions.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
                    };
                });
            #endregion

            //添加跨域策略
            app.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination"));
            });
            //设置Json返回的日期格式
            app.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
        }
    }
}
