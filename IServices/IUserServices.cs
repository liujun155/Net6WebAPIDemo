using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IUserServices : IRepository<User>
    {
        Task<List<User>> GetUsers();

        Task<User> GetUserById(int id);

        Task<ApiResult<object>> AddUser(UserDto user);

        Task<bool> DeleteUser(int id);

        Task<ApiResult<object>> Login(LoginDto account);
    }
}
