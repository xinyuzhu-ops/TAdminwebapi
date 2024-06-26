using xzhuWebApi.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//调用注册扩展类
builder.Register();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//鉴权和授权
app.UseAuthentication();
app.UseAuthorization();

//使用跨域策略
app.UseCors("CorsPolicy");


app.MapControllers();

app.Run();
