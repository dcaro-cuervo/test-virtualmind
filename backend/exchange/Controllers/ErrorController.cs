using System;
using System.Net;
using exchange.Helper;
using Microsoft.AspNetCore.Mvc;

namespace exchange.Controllers
{
    [Route("/errors")]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        [Route("{code}")]
        public IActionResult Error(int code)
        {
            HttpStatusCode parsedCode = (HttpStatusCode)code;
            ApiError error = new ApiError(code, parsedCode.ToString());

            return new ObjectResult(error);
        }
    }
}
