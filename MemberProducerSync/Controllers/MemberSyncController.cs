using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberProducerSync.Model;
using MemberProducerSync.Producer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MemberProducerSync.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MemberSyncController : Controller
    {
        private readonly MemberProducer _producer = new MemberProducer();

        [HttpPost]
        public async Task<IActionResult> SyncMember([FromBody]MemberModel member)
        {
            return await _producer.Send(JsonConvert.SerializeObject(member));
        }
    }
}