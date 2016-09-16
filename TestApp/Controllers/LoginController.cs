using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;
using TestApp.Models;
using System.Security.Cryptography;
using System.Text;
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
            /*if (email != null)
            {
                Customer customer = new Customer()
                {
                    email = HttpUtility.HtmlEncode(email),
                    pswrd = HttpUtility.HtmlEncode(pswrd),
                    //Date = DateTime.UtcNow
                };*/
            string response = DbLogin(email, pswrd);
            //if(response == OK) then proceed
            string ip = HttpContext.Current.Request.UserHostAddress;
            string userAgent = Request.Headers.UserAgent.ToString();
            //string time = Request.Headers.Date.ToString();
            long ticks = 321541646584;
            string token = GenerateToken(email, pswrd, ip, userAgent, ticks);
            return token;

        }
        private const string _alg = "HmacSHA256";
        private const string _salt = "rz8LuOtFBXphj9WQfvFh";

        public static string GenerateToken(string username, string password, string ip, string userAgent, long ticks)
        {
            string hash = string.Join(":", new string[] { username, ip, userAgent, ticks.ToString() });
             string hashLeft = "";
             string hashRight = "";
             using (HMAC hmac = HMACSHA256.Create(_alg))
             {
                 hmac.Key = Encoding.UTF8.GetBytes(GetHashedPassword(password));
                 hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));
                 hashLeft = Convert.ToBase64String(hmac.Hash);
                 hashRight = string.Join(":", new string[] { username, ticks.ToString() });
             }
             return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
           
        }

        public static string GetHashedPassword(string password)
        {
            string key = string.Join(":", new string[] { password, _salt });
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                // Hash the key.
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                return Convert.ToBase64String(hmac.Hash);
                
            }
        }
        public string DbLogin(string username, string password )
        {
            //Connection String Here (Connection Establishment)
            //  SqlConnection conn = new SqlConnection(Catalog = ZIPscentral1; Persist Security Info = False; User ID = { your_username }; Password ={ your_password}; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Authentication = "Active Directory Password");
            SqlCommand cmd = new SqlCommand("TestProcedure", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add( new SqlParameter("@CustomerID", username));
            cmd.Parameters.Add( new SqlParameter("@password", password));
            SqlDataReader rdr = cmd.ExecuteReader();
            return "response"; // from db
        }
    }
}
