/**************************************************************
 * 命名空间：Services
 * 类名称：Repository
 * 文件名：Repository
 * 创建时间：2021/12/7 14:40:21
 * 创建人：LiuJun
 * 创建说明：
 ***************************************************************
 * 修改人：
 * 修改时间：
 * 修改说明：
***************************************************************/

using SqlSugar;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// 仓储类
    /// </summary>
    public class Repository<T> : SimpleClient<T> where T : class, new()
    {
        public Repository(ISqlSugarClient context = null) : base(context)//注意这里要有默认值等于null
        {

            //base.Context = context;//ioc注入的对象
            base.Context = DbScoped.SugarScope; //SqlSugar.Ioc这样写
            // base.Context=DbHelper.GetDbInstance()当然也可以手动去赋值
        }

        /// <summary>
        /// 扩展方法，自带方法不能满足的时候可以添加新方法
        /// </summary>
        /// <returns></returns>
        public List<T> CommQuery(string json)
        {
            //base.Context.Queryable<T>().ToList();可以拿到SqlSugarClient 做复杂操作
            return null;
        }

    }
}