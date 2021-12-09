/**************************************************************
 * 命名空间：Models.Dtos
 * 类名称：LoginDto
 * 文件名：LoginDto
 * 创建时间：2021/12/8 17:45:12
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

namespace Models
{
    /// <summary>
    /// 登录用户DTO
    /// </summary>
    public class LoginDto
    {
        public string Account { get; set; }

        public string Password { get; set; }
    }
}