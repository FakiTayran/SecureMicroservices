using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        [HttpGet("Authorize")]
        public IActionResult Authorize(
             string response_type, //authorization flow type
            string client_id,  //client id
            string redirect_url,
            string scope, // what info i want = email,grandma,tel
            string state // random string generated to confirm that we are going to back to the same client
            )
        {
            // ?a=foo&b=bar
            var query = new QueryBuilder();
            query.Add("redirectUri", redirect_url);
            query.Add("state", state);
            return new JsonResult(query);
        }

        [HttpPost]
        public IActionResult Authorize(
            string username,
            string redirectUri,
            string state)
        {
            const string code = "asbdasdasdasdasd";
            return Redirect("");
        }

        [HttpGet("Token")]
        public IActionResult Token()
        {
            return null;
        }


    }
}
