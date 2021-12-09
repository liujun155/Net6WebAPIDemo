/**************************************************************
 * 命名空间：Services
 * 类名称：UserRoleServices
 * 文件名：UserRoleServices
 * 创建时间：2021/12/8 17:07:10
 * 创建人：LiuJun
 * 创建说明：
 ***************************************************************
 * 修改人：
 * 修改时间：
 * 修改说明：
***************************************************************/

using IServices;
using Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// 用户_角色业务类
    /// </summary>
    public class UserRoleServices : Repository<User_Role>, IUserRoleServices
    {
        public async Task<List<string>> GetRolesByUserIdAsync(int userId)
        {
            var list = await base.Context.Queryable<User_Role, Role>((ur, r) => new JoinQueryInfos(
                 JoinType.Left, ur.RoleId == r.Id))
                .Where(ur => ur.UserId == userId)
                .Select((ur, r) => r.Code)
                .ToListAsync();
            return list;
        }
    }
}