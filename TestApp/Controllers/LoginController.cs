using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestApp.Models;

namespace TestApp.Controllers
{
    public class LoginController : ApiController
    {
        public class AuthRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }
        [HttpPost]
        [ActionName("Login")]
        public string PostLogin([FromBody] AuthRequest req)
        {
            string email = req.username;
            string pswrd = req.password;
            if (email != null)
            {
                Customer customer = new Customer()
                {
                    email = HttpUtility.HtmlEncode(email),
                    pswrd = HttpUtility.HtmlEncode(pswrd),
                    //Date = DateTime.UtcNow
                };

                //var id = Guid.NewGuid();
                //updates[id] = update;

           /*     var response = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(customer.email)
                };*/
             //   response.Headers.Location =
              //      new Uri(Url.Link("DefaultApi", new { action = "status", id = id }));
                return email+pswrd;
            }
            else
            {
                return "Bad Req";
            }

        }
    }
}
