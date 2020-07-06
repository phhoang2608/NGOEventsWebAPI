using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    public class GenerateJWTTokenController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Authenticate(user model)
        {
            using (var context = new NGOEventsEntities())
            {
                UserRequest userRequest = new UserRequest { };
                userRequest.Username = model.email.ToLower();
                userRequest.Password = model.password;
                bool isUsernamePasswordValid = false;

                if (model != null)
                {
                    isUsernamePasswordValid = context.users.Any(x => x.email == model.email && x.password == model.password);
                }
                //if credentials are valid
                if (isUsernamePasswordValid)
                {
                    var userDetails = context.users.Where(x => x.email == model.email && x.password == model.password).FirstOrDefault();
                    int id = userDetails.userID;
                    string role = userDetails.role;
                    return Ok(createToken(model, id, role));
                }
                else
                {
                    // if credentials are not valid send unauthorized status code in response

                    return Content(HttpStatusCode.NotFound, "Wrong username and/or password!");
                }
            }
        }

        private JwtPackage createToken(user users, int id, string role)
            {
                //Set issued at date
                DateTime issuedAt = DateTime.UtcNow;
                //set the time when it expires
                DateTime expires = DateTime.UtcNow.AddDays(7);

                //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
                var tokenHandler = new JwtSecurityTokenHandler();

                //create a identity and add claims to the user which we want to log in
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, users.email)
            });

                const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
                var now = DateTime.UtcNow;
                var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
                var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


                //create the jwt
                var token =
                    (JwtSecurityToken)
                        tokenHandler.CreateJwtSecurityToken(
                            issuer: "https://localhost:44372",
                            audience: "https://localhost:44372",
                            subject: claimsIdentity,
                            notBefore: issuedAt,
                            expires: expires,
                            signingCredentials: signingCredentials
                            );
                var tokenString = tokenHandler.WriteToken(token);
            return new JwtPackage()
            {
                Token = tokenString,
                ID = id,
                Role = role
            };
        }
        }
}
public class JwtPackage
{
    public string Token { get; set; }
    public int ID { get; set; }
    public string Role { get; set; }
}
