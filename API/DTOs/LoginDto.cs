using System;

namespace API.DTOs;
// (Model)  يستقبل بيانات تسجيل الدخول من المستخدم

public class LoginDto
{
    public string Email { get; set; } = "";

     public string Password { get; set; } = "";
    
}
