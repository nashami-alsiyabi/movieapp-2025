using API.Data; // كلاس
using API.Entityes; // كلاس 
using API.Interfaces;
using Microsoft.AspNetCore.Authorization; // التحكم بالصلاحيات(API)
using Microsoft.AspNetCore.Mvc;//عشان نقدر نستخدم [HttpPost], Controller.

namespace API.Controllers
{
   [Authorize]
    public class MembersController(IMemberRepository memberRepository) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        {
           

            return Ok (await memberRepository.GetMembersAsync());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(string id)
        {
            var member = await memberRepository.GetMemberByIdAsync(id);

            if (member == null) return NotFound();

            
            return member  ;
        }
        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            return Ok (await memberRepository.GetPhotosForMemberAsync(id));      
        }
    }
}
