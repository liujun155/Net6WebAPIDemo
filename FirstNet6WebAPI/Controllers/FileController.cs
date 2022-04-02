using Microsoft.AspNetCore.Mvc;

namespace FirstNet6WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">来自form表单的文件信息</param>
        /// <returns></returns>
        //[HttpPost]
        //public IActionResult Post([FromForm] IFormFile file)
        //{
        //    if (file.Length <= this._pictureOptions.MaxSize)//检查文件大小
        //    {
        //        var suffix = Path.GetExtension(file.FileName);//提取上传的文件文件后缀
        //        if (this._pictureOptions.FileTypes.IndexOf(suffix) >= 0)//检查文件格式
        //        {
        //            CombineIdHelper combineId = new CombineIdHelper();//我自己的combine id生成器
        //            using (FileStream fs = System.IO.File.Create($@"{this._pictureOptions.ImageBaseUrl}\{combineId.CreateId()}{suffix}"))//注意路径里面最好不要有中文
        //            {
        //                file.CopyTo(fs);//将上传的文件文件流，复制到fs中
        //                fs.Flush();//清空文件流
        //            }
        //            return StatusCode(200, new { newFileName = $"{combineId.LastId}{suffix}" });//将新文件文件名回传给前端
        //        }
        //        else
        //            return StatusCode(415, new { msg = "不支持此文件类型" });//类型不正确
        //    }
        //    else
        //        return StatusCode(413, new { msg = $"文件大小不得超过{this._pictureOptions.MaxSize / (1024f * 1024f)}M" });//请求体过大，文件大小超标
        //}
    }
}
