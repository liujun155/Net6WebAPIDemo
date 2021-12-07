/**************************************************************
 * 命名空间：Models
 * 类名称：User
 * 文件名：User
 * 创建时间：2021/12/6 17:37:32
 * 创建人：LiuJun
 * 创建说明：
 ***************************************************************
 * 修改人：
 * 修改时间：
 * 修改说明：
***************************************************************/

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 用户 数据模型
    /// </summary>
    [SugarTable("Users")]
    public class User
    {
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "Name")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "CreateTime", IsOnlyIgnoreInsert = true)]//IsOnlyIgnoreInsert设置后会插入默认值
        public DateTime CreateTime { get; set; }
    }
}