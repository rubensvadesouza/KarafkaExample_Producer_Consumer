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
    public class MemberLegacyController : Controller
    {
        private readonly MemberSqlService _service = new MemberSqlService();

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]MemberModel member)
        {
            return null;
        }
    }
}