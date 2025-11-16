using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;// (للتعامل مع SQL)
using Microsoft.IdentityModel.Tokens;
using System.Text;// لتحويل النصوص إلى Bytes قبل التشفير.
using Microsoft.AspNetCore.Authentication.JwtBearer;// عشان نفعل JWT Authentication

var builder = WebApplication.CreateBuilder(args);// ينشئ تطبيق ويب جديد

// Add services to the container.

builder.Services.AddControllers();// فعّل دعم API Controllers

builder.Services.AddDbContext<AppDbContext>(opt =>// إعداد قاعدة البيانات
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));//مكان تخزين وقراءة الإعدادات
});
builder.Services.AddCors(); // Cross-Origin Resource Sharing لوصول API للمتصفح
builder.Services.AddScoped<ITokenService, TokenService>();// لكل HTTP Request ينشئ instance جديدة ويستخدمها داخل نفس الطلب فقط.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//يفعّل نظام Authentication في المشروع.
.AddJwtBearer(Options =>
{
    var tokenKey = builder.Configuration["TokenKey"] // Configuration هي: المكان اللي يحفظ كل الإعدادات الخاصة بالمشروع
    ?? throw new Exception("Token Key not found - Program.cs");

    Options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,// “تحقق بأن التوكن موقع باستخدام المفتاح اللي عندي
        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenKey)),
        ValidateIssuer = false, // السيرفر اللي أصدر التوكن
        ValidateAudience = false//ن المفروض يستقبل هذا التوكن؟
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("http://localhost:4200", "https://localhost:4200"));
// AllowAnyHeader() → يسمح بأي Header.

//AllowAnyMethod() → يسمح بأي HTTP Method (GET, POST, PUT, DELETE…).

//WithOrigins(...) → يسمح فقط للطلبات اللي جاية من:

//http://localhost:4200,https://localhost:4200


app.UseAuthentication();// يقرأ الـ Authorization Header.ويحدد اسم المستخدم
app.UseAuthorization();//

app.MapControllers();// يخلي ASP.NET يطلع Endpoints بناءً على Controllers اللي عندك.

app.Run();//يبدأ تشغيل السيرفر.
