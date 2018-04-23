using System.Threading.Tasks;
using MemberProducerSync.EF;
using MemberProducerSync.MemberService;
using MemberProducerSync.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemberProducerSync.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MemberLegacyController : Controller
    {
        private readonly MemberEntityService _service;

        public MemberLegacyController(MemberContext context)
        {
            _service = new MemberEntityService(context);
        }


        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]MemberModel member)
        {
            _service.InsertOrUpdate(member);

            return Ok();
        }
    }
}