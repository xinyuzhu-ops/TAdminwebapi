using Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.ApiRes;
using Model.Dto.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CustomJwtService : ICustomJwtService
    {
        private readonly JWTToken _wTToken;

        public CustomJwtService(IOptionsMonitor<JWTToken> wTToken)
        {
            _wTToken = wTToken.CurrentValue;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetToken(UserRes user)
        {
            var claims = new[]
            {
                new Claim("Id",user.Id.ToString()),
                new Claim("NickName",user.NickName),
                new Claim("Name",user.Name),
                new Claim("UserType",user.UserType.ToString()),
            };
            //加密key
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_wTToken.SecurityKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _wTToken.Issuer,
                audience: _wTToken.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),//10分钟有效期
                signingCredentials: creds);

            string returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
        }
    }

}
