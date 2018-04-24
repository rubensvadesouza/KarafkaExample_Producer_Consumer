using MemberProducerSync.EF;
using MemberProducerSync.MemberService;
using MemberProducerSync.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

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
        public IActionResult Insert([FromBody]string member)
        {
            var m = JsonConvert.DeserializeObject<MemberModel>(member);
            if (m != null)
                _service.InsertOrUpdate(m);

            return Ok();
        }

        [HttpGet()]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }
    }
}