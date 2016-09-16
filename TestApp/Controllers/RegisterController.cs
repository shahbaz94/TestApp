using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;

namespace TestApp.Controllers
{
    public class RegisterController : ApiController
    {
        public class AuthRequest
        {
            public string username { get; set; }
            public string password { get; set; }
            public string phone_num { get; set; }
            public string f_name { get; set; }
            public string l_name { get; set; }
        }
        [HttpPost]
        [ActionName("Register")]
        public string PostRegister([FromBody] AuthRequest req)
        {
            string fname = req.f_name;
            string lname = req.l_name;
            string username = req.username;
            string pswrd = req.password;
            string phone = req.phone_num;
            return fname + " " + lname + " " + username + " " + pswrd + " " + phone;
        }
        public string DbRegister(string fname, string lname, string username, string pswrd, string phone_num)
        {
            //Connection String Here (Connection Establishment)
            //  SqlConnection conn = new SqlConnection(Catalog = ZIPscentral1; Persist Security Info = False; User ID = { your_username }; Password ={ your_password}; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Authentication = "Active Directory Password");
            SqlCommand cmd = new SqlCommand("registerProcedure", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CustomerID", username));
            cmd.Parameters.Add(new SqlParameter("@password", pswrd));
            SqlDataReader rdr = cmd.ExecuteReader();
            return "response"; // from db
        }
    }
}
