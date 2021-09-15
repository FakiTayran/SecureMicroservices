using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        [HttpGet("Authorize")]
        public IActionResult Authorize(
             string response_type, //authorization flow type
            string client_id,  //client id
            string redirect_uri,
            string scope, // what info i want = email,grandma,tel
            string state // random string generated to confirm that we are going to back to the same client
            )
        {
            // ?a=foo&b=bar
            var username = "fakininfirmasi";
            var query = new QueryBuilder();
            query.Add("username", username);
            query.Add("redirectUri", redirect_uri);
            query.Add("state", state);
            return Callback(username, redirect_uri, state);
        }

        [HttpPost("Callback")]
        public IActionResult Callback(
            string username,
            string redirect_uri,
            string state)
        {
            string code = "asbdasdasdasdasd";
            var query = new QueryBuilder();
            query.Add("code", code);
            query.Add("state", state);
            return Token("authorization_code",code,redirect_uri,"client_id");
        }

        [HttpGet("Token")]
        public  IActionResult Token(
            string grant_type, //flow of access_token request
            string code,   //confirmation of the authentication process
            string redirect_uri,  
            string client_id
            )
        {
            var claims = new[]
                     {
                new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                new Claim("granny", "cookie")
            };

            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audiance,
                claims,
                notBefore: DateTime.Now,
                expires: grant_type == "authorization_code"
                    ? DateTime.Now.AddMinutes(5)
                    : DateTime.Now.AddMilliseconds(1),
                signingCredentials);

            var access_token = new JwtSecurityTokenHandler().WriteToken(token);

            var responseObject = new
            {
                access_token,
                token_type = "Bearer",
                raw_claim = "oauthTutorial",
                refresh_token = "RefreshTokenSampleValueSomething77"
            };

            var responseJson = JsonConvert.SerializeObject(responseObject);
            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);

            return Redirect(redirect_uri);
        }


    }
}
