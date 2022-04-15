using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FirstNet6WebAPI.Controllers
{
    /// <summary>
    /// 文件接口类
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly PictureOptions _pictureOptions;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public FileController(IOptions<PictureOptions> options)
        {
            _pictureOptions = options.Value;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files">来自form表单的文件信息</param>
        /// <returns></returns>
        [HttpPost("UploadImage")]
        public IActionResult UploadImage([FromForm]IList<IFormFile> files)
        {
            IFormFile file = files[0];
            Common.Helper.LogUtil.Info("进入上传接口");
            if (file.Length <= _pictureOptions.MaxSize)//检查文件大小
            {
                var extensionName = Path.GetExtension(file.FileName);//提取上传的文件文件后缀
                if (this._pictureOptions.FileTypes.IndexOf(extensionName) >= 0)//检查文件格式
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    using (FileStream fs = System.IO.File.Create($@"{this._pictureOptions.ImageBaseUrl}\{fileName}{extensionName}"))//注意路径里面最好不要有中文
                    {
                        file.CopyTo(fs);//将上传的文件文件流，复制到fs中
                        fs.Flush();//清空文件流
                    }
                    return StatusCode(200, new { newFileName = $"{fileName}{extensionName}" });//将新文件文件名回传给前端
                }
                else
                    return StatusCode(415, new { msg = "不支持此文件类型" });//类型不正确
            }
            else
                return StatusCode(413, new { msg = $"文件大小不得超过{this._pictureOptions.MaxSize / (1024f * 1024f)}M" });//请求体过大，文件大小超标
        }
    }
}
