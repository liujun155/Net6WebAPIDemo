/**************************************************************
 * 命名空间：Services
 * 类名称：UserServices
 * 文件名：UserServices
 * 创建时间：2021/12/7 10:01:19
 * 创建人：LiuJun
 * 创建说明：
 ***************************************************************
 * 修改人：
 * 修改时间：
 * 修改说明：
***************************************************************/

using Common;
using IServices;
using Models;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UserServices : Repository<User>, IUserServices
    {
        public async Task<List<User>> GetUsers()
        {
            //var users = DbScoped.Sugar.Queryable<User>().ToList();
            //return users;
            return await this.GetListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            //var user = DbScoped.Sugar.Queryable<User>().InSingle(id);
            //return user;
            return await this.GetByIdAsync(id);
        }

        public async Task<ApiResult<object>> AddUser(UserDto user)
        {
            if (user == null || string.IsNullOrEmpty(user.Name)) return new ApiResult<object>(0, "参数错误", user);
            User model = new User() { Name = user.Name };
            var isAny = await this.IsAnyAsync(x => x.Name == user.Name);
            if (isAny) return new ApiResult<object>(0, "名称重复", user);
            var isSuccess = await this.InsertAsync(model);
            return new ApiResult<object>(isSuccess);
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await this.DeleteByIdAsync(id);
        }
    }
}