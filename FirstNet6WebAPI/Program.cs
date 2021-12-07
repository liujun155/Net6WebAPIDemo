using FirstNet6WebAPI;
using IServices;
using Microsoft.OpenApi.Models;
using Services;
using SqlSugar.IOC;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//添加接口依赖注入
builder.Services.AddSingleton<IUserServices, UserServices>();
#region SqlSugar配置
// SqlSugarScope用单例AddSingleton  单例
// SqlSugarClient用 AddScoped  每次请求一个实例

SugarIocServices.AddSqlSugar(new IocConfig()
{
    //ConfigId="db01"  多租户用到
    ConnectionString = "server=.;uid=sa;pwd=123456;database=Net6Study",
    DbType = IocDbType.SqlServer,
    IsAutoCloseConnection = true//自动释放
}); //多个库就传List<IocConfig>
//builder.Services.AddSqlsugarSetup(Configuration);

//配置参数
SugarIocServices.ConfigurationSugar(db =>
{
    db.Aop.OnLogExecuting = (sql, p) =>
    {
        Console.WriteLine(sql);
    };
    //设置更多连接参数
    //db.CurrentConnectionConfig.XXXX=XXXX
});
#endregion

#region   配置跨域
builder.Services.AddCors(c =>
{
    c.AddPolicy("Cors", policy =>
    {
        policy
              .AllowAnyOrigin()
              .AllowAnyHeader()//Ensures that the policy allows any header.
              .AllowAnyMethod();
    });
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Cors");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
