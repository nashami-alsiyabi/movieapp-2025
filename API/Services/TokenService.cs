using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entityes;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

//  (IConfiguration)  عشان نقرأ الـ TokenKey من appsettings.json.
public class TokenService(IConfiguration config) : ITokenService
{
 public string CreataToken(AppUser user)
    {// قراءة TokenKey من إعدادات التطبيق
        var tokenKey = config["TokenKey"] ?? throw new Exception("Connot get token key "); 

        if (tokenKey.Length < 64)
            throw new Exception("Yoour token Key needs to be >= 64 characters");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
// SigningCredentials = بيانات التوقيع

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            //ClaimsIdentity = بطاقة تعريف المستخدم داخل JWT.
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
        //يحول التوكن إلى  string
// يضيف الفواصل والنقاط
    }

}
