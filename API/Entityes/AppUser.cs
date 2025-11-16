using System;

namespace API.Entityes;
//  namespace تنظم الملفات داخل المشروع


//وهي تمثل "المستخدم" في قاعدة البيانات.
public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
// Guid إنشاء رقم فريد
//يصير نص (string) بدل قيمة GUID.
    public required string DisplayName { get; set; }

    public required string Email { get; set; }

    // this 
    public required byte[] PasswordHash { get; set; }
    //كلمة المرور بعد ما يتم تشفيرها (Hash).
    public required byte[] PasswordSalt { get; set; }
   


// عشان ما نخزن كلمة المرور الحقيقية في قاعدة البيانات.
//نخزن فقط النتيجة المشفّرة (hash) + الـ salt.
//حتى لو أحد اخترق القاعدة، ما يقدر يعرف الباسورد الحقيقي.
}
