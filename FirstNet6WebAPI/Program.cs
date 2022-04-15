using AutoMapper;
using Common;
using FirstNet6WebAPI;
using IServices;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services;
using SqlSugar.IOC;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ConfigInfo.SqlServerConn = builder.Configuration.GetValue<string>("SqlServerConn");
ConfigInfo.JwtSecret = builder.Configuration.GetValue<string>("Jwt:Secret");
ConfigInfo.JwtRSecret = builder.Configuration.GetValue<string>("Jwt:RSecret");
ConfigInfo.Issuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
ConfigInfo.Audience = builder.Configuration.GetValue<string>("Jwt:Audience");

ILoggerRepository repository = LogManager.CreateRepository("LogRepository");
XmlConfigurator.Configure(repository, new FileInfo("Config/log4net.config"));

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

    //生成注释
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    #region 配置jwt
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
    #endregion
});

//添加接口依赖注入IOC
builder.Services.AddSingleton<IUserServices, UserServices>();
builder.Services.AddSingleton<IRoleServices, RoleServices>();
builder.Services.AddSingleton<IUserRoleServices, UserRoleServices>();

//图片上传配置项
//配置项转强类型对象
builder.Services.Configure<PictureOptions>(builder.Configuration.GetSection("PictureOptions"));
//var config = new ConfigurationBuilder()
//    .AddInMemoryCollection()
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//    .Build();
//var service = new ServiceCollection()
//    .AddOptions()
//    .Configure<PictureOptions>(config.GetSection("PictureOptions"))
//    .AddTransient<PictureOptions>()
//    .BuildServiceProvider();

//身份认证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secretByte = Encoding.UTF8.GetBytes(ConfigInfo.JwtSecret);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = ConfigInfo.Issuer,

                        ValidateAudience = true,
                        ValidAudience = ConfigInfo.Audience,

                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
                            //context.HandleResponse();

                            //自定义自己想要返回的数据结果，我这里要返回的是Json对象，通过引用Newtonsoft.Json库进行转换

                            //自定义返回的数据类型
                            //context.Response.ContentType = "text/plain";
                            ////自定义返回状态码，默认为401 我这里改成 200
                            ////context.Response.StatusCode = StatusCodes.Status200OK;
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            //输出Json数据结果
                            context.Response.WriteAsync("expired");
                            return Task.FromResult(0);
                        },
                        //403
                        OnForbidden = context =>
                        {
                            //context.Response.ContentType = "text/plain";
                            ////自定义返回状态码，默认为401 我这里改成 200
                            ////context.Response.StatusCode = StatusCodes.Status200OK;
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            //输出Json数据结果
                            context.Response.WriteAsync("expired");
                            return Task.FromResult(0);
                        }

                    };
                });

#region SqlSugar配置
// SqlSugarScope用单例AddSingleton  单例
// SqlSugarClient用 AddScoped  每次请求一个实例

SugarIocServices.AddSqlSugar(new IocConfig()
{
    //ConfigId="db01"  多租户用到
    ConnectionString = ConfigInfo.SqlServerConn,
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

#region 配置跨域
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

#region AutoMapper配置
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
// 开发环境时启用swagger
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("Cors");
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
