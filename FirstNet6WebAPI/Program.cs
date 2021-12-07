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

//��ӽӿ�����ע��
builder.Services.AddSingleton<IUserServices, UserServices>();
#region SqlSugar����
// SqlSugarScope�õ���AddSingleton  ����
// SqlSugarClient�� AddScoped  ÿ������һ��ʵ��

SugarIocServices.AddSqlSugar(new IocConfig()
{
    //ConfigId="db01"  ���⻧�õ�
    ConnectionString = "server=.;uid=sa;pwd=123456;database=Net6Study",
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

#region   ���ÿ���
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
