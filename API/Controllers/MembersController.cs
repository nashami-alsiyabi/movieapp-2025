using API.Data; // كلاس
using API.Entityes; // كلاس 
using Microsoft.AspNetCore.Authorization; // التحكم بالصلاحيات(API)
using Microsoft.AspNetCore.Mvc;//عشان نقدر نستخدم [HttpPost], Controller.
using Microsoft.EntityFrameworkCore;//للتعامل مع قاعدة البيانات.

namespace API.Controllers
{
   [AllowAnonymous] 

    public class MembersController(AppDbContext context) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members =  await context.Users.ToListAsync();

            return members;
        }
[AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            var members = await context.Users.FindAsync(id);

            if (members == null) return NotFound();

            
            return members  ;
        }
    }
}
