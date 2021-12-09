/**************************************************************
 * 命名空间：Services
 * 类名称：RoleServices
 * 文件名：RoleServices
 * 创建时间：2021/12/8 17:06:03
 * 创建人：LiuJun
 * 创建说明：
 ***************************************************************
 * 修改人：
 * 修改时间：
 * 修改说明：
***************************************************************/

using IServices;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// 角色业务类
    /// </summary>
    public class RoleServices : Repository<Role>, IRoleServices
    {
    }
}