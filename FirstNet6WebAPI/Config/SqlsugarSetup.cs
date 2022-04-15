using SqlSugar;

namespace FirstNet6WebAPI
{
    /// <summary>
    /// SqlSugarScope用单例AddSingleton  单例
    /// SqlSugarClient用 AddScoped  每次请求一个实例
    /// </summary>
    public static class SqlsugarSetup
    {
        public static void AddSqlsugarSetup(this IServiceCollection services, IConfiguration configuration,
        string dbName = "db_master")
        {
            SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = SqlSugar.DbType.SqlServer,
                ConnectionString = configuration.GetConnectionString(dbName),
                IsAutoCloseConnection = true,
            },
                db =>
                {
                //单例参数配置，所有上下文生效
                db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                    //Console.WriteLine(sql);//输出sql
                };
                });
            services.AddSingleton<ISqlSugarClient>(sqlSugar);//这边是SqlSugarScope用AddSingleton
        }
    }
}
