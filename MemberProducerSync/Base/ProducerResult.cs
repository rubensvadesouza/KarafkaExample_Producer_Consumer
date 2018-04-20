using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberProducerSync.Base
{
    public class ProducerResult : ActionResult
    {
        public ProducerResult(EnumProducerResult result)
        {
            Result = result;
        }

        public EnumProducerResult Result { get; set; }

        public string Message { get; set; }

        public static ProducerResult Sucess = new ProducerResult(EnumProducerResult.Sucess);

        public static ProducerResult GetError(string error)
        {
            var result = new ProducerResult(EnumProducerResult.Error)
            {
                Message = error
            };

            return result;
        }

        public static ProducerResult GetWarning(string warning)
        {
            var result = new ProducerResult(EnumProducerResult.Error)
            {
                Message = warning
            };

            return result;
        }

        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);
        }


        public override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)Result;

            if (!string.IsNullOrEmpty(Message))
            {
                var xmlBytes = Encoding.ASCII.GetBytes(Message);
                context.HttpContext.Response.Body.WriteAsync(xmlBytes, 0, xmlBytes.Length);
            }

            return base.ExecuteResultAsync(context);
        }
    }
}

