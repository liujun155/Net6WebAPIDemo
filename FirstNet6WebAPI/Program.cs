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

    //����ע��
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    #region ����jwt
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken��Bearer Token",
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

//��ӽӿ�����ע��IOC
builder.Services.AddSingleton<IUserServices, UserServices>();
builder.Services.AddSingleton<IRoleServices, RoleServices>();
builder.Services.AddSingleton<IUserRoleServices, UserRoleServices>();

//ͼƬ�ϴ�������
//������תǿ���Ͷ���
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

//�����֤
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
                            //�˴�����Ϊ��ֹ.Net CoreĬ�ϵķ������ͺ����ݽ�����������ҪŶ������
                            //context.HandleResponse();

                            //�Զ����Լ���Ҫ���ص����ݽ����������Ҫ���ص���Json����ͨ������Newtonsoft.Json�����ת��

                            //�Զ��巵�ص���������
                            //context.Response.ContentType = "text/plain";
                            ////�Զ��巵��״̬�룬Ĭ��Ϊ401 ������ĳ� 200
                            ////context.Response.StatusCode = StatusCodes.Status200OK;
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            //���Json���ݽ��
                            context.Response.WriteAsync("expired");
                            return Task.FromResult(0);
                        },
                        //403
                        OnForbidden = context =>
                        {
                            //context.Response.ContentType = "text/plain";
                            ////�Զ��巵��״̬�룬Ĭ��Ϊ401 ������ĳ� 200
                            ////context.Response.StatusCode = StatusCodes.Status200OK;
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            //���Json���ݽ��
                            context.Response.WriteAsync("expired");
                            return Task.FromResult(0);
                        }

                    };
                });

#region SqlSugar����
// SqlSugarScope�õ���AddSingleton  ����
// SqlSugarClient�� AddScoped  ÿ������һ��ʵ��

SugarIocServices.AddSqlSugar(new IocConfig()
{
    //ConfigId="db01"  ���⻧�õ�
    ConnectionString = ConfigInfo.SqlServerConn,
    DbType = IocDbType.SqlServer,
    IsAutoCloseConnection = true//�Զ��ͷ�
}); //�����ʹ�List<IocConfig>
//builder.Services.AddSqlsugarSetup(Configuration);

//���ò���
SugarIocServices.ConfigurationSugar(db =>
{
    db.Aop.OnLogExecuting = (sql, p) =>
    {
        Console.WriteLine(sql);
    };
    //���ø������Ӳ���
    //db.CurrentConnectionConfig.XXXX=XXXX
});
#endregion

#region ���ÿ���
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

#region AutoMapper����
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
// ��������ʱ����swagger
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
