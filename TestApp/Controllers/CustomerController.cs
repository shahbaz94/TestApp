using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestApp.Models;

namespace TestApp.Controllers
{
    public class CustomerController : ApiController
    {
        Customer[] customers = new Customer[]
        {
            new Customer { fname = "Shahbaz", lname = "Hussain", email = "shahbaz_se@hotmail.com", pswrd = "12345" },
            new Customer { fname = "Fiaz", lname = "Hussain", email = "fiaz@hotmail.com", pswrd = "45678" },
            new Customer { fname = "Ayyan", lname = "Usman", email = "ayyan@hotmail.com", pswrd = "7894" },
        };
        
        
        
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string Register(string fname, string lname, string email, string pswrd)
        {
            return "Hello " + fname  + lname +email+pswrd;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public string Login(string email, string pswrd)
        {
            return "token";
        }
    }
}
