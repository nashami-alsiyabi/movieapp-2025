
using System.Security.Cryptography; // لتشفير كلمات المرور
using System.Text;// لتحويل النصوص إلى Bytes قبل التشفير.
using API.Data;//كلاس
using API.DTOs; //كلاس
using API.Entityes; // كلاس
using API.Extensions;// كلاس
using API.Interfaces;// كلاس
using Microsoft.AspNetCore.Mvc; //عشان نقدر نستخدم [HttpPost], Controller.
using Microsoft.EntityFrameworkCore; //للتعامل مع قاعدة البيانات.

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await EmailExists(registerDto.Email)) return BadRequest("Email taken");
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key

        };
        context.Users.Add(user);
        await context.SaveChangesAsync(); // هنا يتم الحفظ  داخل قاعدة البيانات

        return user.ToDto(tokenService);
    }
        [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null) return Unauthorized("Invalid email address");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (var i = 0; i < ComputeHash.Length; i++)
        {
            if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }
        return user.ToDto(tokenService);
    }
    private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
}
