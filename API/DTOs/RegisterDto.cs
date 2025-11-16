using System;
using System.ComponentModel.DataAnnotations; // الـ Validation Attributes (شروط الإدخال).

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; } = "";

    [Required]
    [EmailAddress] // يتأكد إن المدخل شكل إيميل صحيح
     public  string Email { get; set; }= "";

    [Required]
    [MinLength(4)]
    public string Password { get; set; } = "";

}
