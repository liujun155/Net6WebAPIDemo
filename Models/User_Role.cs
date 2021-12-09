/**************************************************************
 * 命名空间：Models
 * 类名称：User_Role
 * 文件名：User_Role
 * 创建时间：2021/12/8 16:34:32
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
    /// 用户-角色关联表
    /// </summary>
    [SugarTable("User_Role")]
    public class User_Role
    {
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true)]
        public int Id { get; set; }
        [SugarColumn(ColumnName = "UserId")]
        public int UserId { get; set; }
        [SugarColumn(ColumnName = "RoleId")]
        public int RoleId { get; set; }
    }
}