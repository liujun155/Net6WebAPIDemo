/**************************************************************
 * 命名空间：Common.AuthHelper
 * 类名称：JwtToken
 * 文件名：JwtToken
 * 创建时间：2021/12/9 11:05:31
 * 创建人：LiuJun
 * 创建说明：
 ***************************************************************
 * 修改人：
 * 修改时间：
 * 修改说明：
***************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AuthHelper
{
    public class TokenModelJwt
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public List<string> RoleList { get; set; } = new List<string>();
    }
}