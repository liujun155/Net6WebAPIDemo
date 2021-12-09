/**************************************************************
 * 命名空间：Common
 * 类名称：ConfigInfo
 * 文件名：ConfigInfo
 * 创建时间：2021/12/8 17:36:54
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

namespace Common
{
    /// <summary>
    /// 配置项
    /// </summary>
    public static class ConfigInfo
    {
        public static string SqlServerConn { get; set; }
        public static string JwtSecret { get; set; }
        public static string JwtRSecret { get; set; }
        public static string Issuer { get; set; }
        public static string Audience { get; set; }
    }
}