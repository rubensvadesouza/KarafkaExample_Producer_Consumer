﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberProducerSync.MemberService;
using MemberProducerSync.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemberProducerSync.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MemberNextController : Controller
    {
        private readonly MemberMongoService _service = new MemberMongoService();

        [HttpPost]
        public IActionResult Insert([FromBody]MemberModel model)
        {
            _service.InsertMember(model);
            return Ok();
        }

        [HttpGet()]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _service.FindAsync(id);
            return Ok(result);
        }

    }
}