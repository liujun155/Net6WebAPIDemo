/**************************************************************
 * 命名空间：Common.AuthHelper
 * 类名称：JwtHelper
 * 文件名：JwtHelper
 * 创建时间：2021/12/9 10:57:16
 * 创建人：LiuJun
 * 创建说明：
 ***************************************************************
 * 修改人：
 * 修改时间：
 * 修改说明：
***************************************************************/

using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Common.AuthHelper
{
    public class JwtHelper
    {
        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public static string CreateToken(JwtToken info)
        {
            var claims = new List<Claim>()
            {
                new Claim("UserName",info.UserName),
                new Claim("UserId",info.UserId)
            };
            //赋予角色
            //foreach (var role in info.RoleList)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}
            claims.AddRange(info.RoleList.Select(s => new Claim(ClaimTypes.Role, s)));
            var secret = ConfigInfo.JwtSecret;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: ConfigInfo.Issuer,
                audience: ConfigInfo.Audience,
                claims: claims,
                expires: DateTime.Now.AddSeconds(20), //20分钟有效期
                signingCredentials: credentials);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenStr;
        }



        /// <summary>
        /// 创建RefreshToken
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public static string CreateRToken(JwtToken info)
        {
            var claims = new List<Claim>()
            {
                new Claim("UserName",info.UserName),
                new Claim("UserId",info.UserId)
            };
            //赋予角色
            //foreach (var role in info.RoleList)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}
            claims.AddRange(info.RoleList.Select(s => new Claim(ClaimTypes.Role, s)));
            var secret = ConfigInfo.JwtRSecret;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: ConfigInfo.Issuer,
                audience: ConfigInfo.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(20), //20分钟有效期
                signingCredentials: credentials);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenStr;
        }
        /// <summary>
        /// 解析token
        /// </summary>
        /// <returns></returns>
        public static JwtToken GetTokenValue(string token)
        {
            var tokenJwt = new JwtToken();
            var validateParameter = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = ConfigInfo.Issuer,
                ValidAudience = ConfigInfo.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigInfo.JwtSecret))
            };
            try
            {
                //校验并解析token
                var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, validateParameter, out SecurityToken validatedToken);//validatedToken:解密后的对象
                var jwtPayload = ((JwtSecurityToken)validatedToken).Payload.SerializeToJson(); //获取payload中的数据 
                tokenJwt = JsonConvert.DeserializeObject<JwtToken>(jwtPayload);
            }
            catch (SecurityTokenExpiredException)
            {
                return null;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            return tokenJwt;
        }

        /// <summary>
        /// 解析Rtoken
        /// </summary>
        /// <returns></returns>
        public static JwtToken GetRTokenValue(string token)
        {
            var tokenJwt = new JwtToken();
            var validateParameter = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = ConfigInfo.Issuer,
                ValidAudience = ConfigInfo.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigInfo.JwtRSecret))
            };
            try
            {
                //校验并解析token
                var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, validateParameter, out SecurityToken validatedToken);//validatedToken:解密后的对象
                var jwtPayload = ((JwtSecurityToken)validatedToken).Payload.SerializeToJson(); //获取payload中的数据 
                tokenJwt = JsonConvert.DeserializeObject<JwtToken>(jwtPayload);
            }
            catch (SecurityTokenExpiredException)
            {
                return null;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            return tokenJwt;
        }
    }
}
