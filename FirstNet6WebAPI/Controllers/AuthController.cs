using Common;
using Common.AuthHelper;
using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace FirstNet6WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IUserRoleServices _userRoleServices;
        public AuthController(IUserServices userServices, IUserRoleServices userRoleServices)
        {
            _userServices = userServices;
            _userRoleServices = userRoleServices;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            var result = await _userServices.Login(user);
            if (result.Code == 1) return new JsonResult(result);

            var userModel = result.Data as User;
            if (userModel == null) return new JsonResult(new ApiResult<object>(false));
            var roleList = await _userRoleServices.GetRolesByUserIdAsync(userModel.Id);
            var jwt = new JwtToken()
            {
                UserName = userModel.Name,
                UserId = userModel.Id.ToString(),
                RoleList = roleList
            };
            var token = JwtHelper.CreateToken(jwt);
            var Rtoken = JwtHelper.CreateRToken(jwt);
            result.Data = token;

            return new JsonResult(new
            {
                token = token,
                rtoken = Rtoken
            });
        }
    }
}
