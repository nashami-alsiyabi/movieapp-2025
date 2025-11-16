using System;
using API.Entityes;

namespace API.Interfaces;

public interface ITokenService // اسم الخدمة اللي مسؤولة عن إنشاء Token للمستخدم
{
    string CreataToken(AppUser user);
}
