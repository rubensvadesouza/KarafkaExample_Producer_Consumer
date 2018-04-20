using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberProducerSync.MemberService;
using MemberProducerSync.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemberProducerSync.Controllers
{
    [Produces("application/json")]
    [Route("api/Member")]
    public class MemberNextController : Controller
    {
        private readonly MemberMongoService _service = new MemberMongoService();

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]MemberModel member)
        {
            return null;
        }
    }
}