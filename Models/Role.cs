/**************************************************************
 * 命名空间：Models
 * 类名称：Roles
 * 文件名：Roles
 * 创建时间：2021/12/8 16:30:44
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
    /// 角色
    /// </summary>
    [SugarTable("Roles")]
    public class Role
    {
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        [SugarColumn(ColumnName = "Code")]
        public string Code { get; set; }
        [SugarColumn(ColumnName = "Name")]
        public string Name { get; set; }
    }
}