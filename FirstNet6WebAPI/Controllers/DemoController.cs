using Common;
using IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using SqlSugar;
using SqlSugar.IOC;

namespace FirstNet6WebAPI.Controllers
{
    /// <summary>
    /// 示例控制器
    /// </summary>
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        readonly IUserServices _userServices;
        public DemoController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpGet("getdata")]
        [Route("getdata/{id}"), HttpGet]
        [HttpGet]
        public string GetData(string id)
        {
            return $"Hello World {id}";
        }
        /// <summary>
        /// 获取数据1
        /// </summary>
        /// <returns></returns>
        //[HttpGet("getdata1")]
        [Route("getdata1"), HttpGet]
        public string GetData1()
        {
            return "Hello World 小红红";
        }

        /// <summary>
        /// 获取日期
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDate")]
        public IActionResult GetDate()
        {
            var date = new
            {
                time = DateTime.Now,
                code = 1,
                msg = "OK"
            };
            return new JsonResult(date);
        }

        /// <summary>
        /// 获取姓名
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        [HttpGet("GetName")]
        public IActionResult GetName(int id, string name)
        {
            var data = new
            {
                id = id,
                name = name
            };
            return new JsonResult(data);
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userServices.GetUsers();
            return new JsonResult(users);
        }

        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userServices.GetUserById(id);
            return new JsonResult(user);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserDto user)
        {
            var result = await _userServices.AddUser(user);
            return new JsonResult(result);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var isSuccess = await _userServices.DeleteUser(id);
            return new JsonResult(new ApiResult<object>(isSuccess));
        }
    }
}
