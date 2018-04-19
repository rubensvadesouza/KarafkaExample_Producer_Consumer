using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemberProducerSync.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MemberController : Controller
    {
    }
}