/**************************************************************
 * 命名空间：Common
 * 类名称：ApiResult
 * 文件名：ApiResult
 * 创建时间：2021/12/7 17:02:55
 * 创建人：LiuJun
 * 创建说明：
 ***************************************************************
 * 修改人：
 * 修改时间：
 * 修改说明：
***************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 接口返回对象
    /// </summary>
    public class ApiResult<T>
    {
        public int Code { get; set; } = 0;
        public string Msg { get; set; } = "Ok";
        public T Data { get; set; }

        public ApiResult(int code, string msg, T data)
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
        }

        public ApiResult(bool isSuccess, string msg = "")
        {
            if(isSuccess)
            {
                Code = 0;
                Msg = msg == "" ? "成功" : msg;
            }
            else
            {
                Code = 1;
                Msg = msg == "" ? "失败" : msg;
            }
        }
    }
}