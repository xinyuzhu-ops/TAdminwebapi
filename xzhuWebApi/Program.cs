using xzhuWebApi.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//����ע����չ��
builder.Register();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//��Ȩ����Ȩ
app.UseAuthentication();
app.UseAuthorization();

//ʹ�ÿ������
app.UseCors("CorsPolicy");


app.MapControllers();

app.Run();
