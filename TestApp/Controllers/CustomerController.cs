using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using TestApp.Models;
using System.Text;
using System.Web;

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
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        public string Post([FromBody]dynamic email)
        {
            string ip = HttpContext.Current.Request.UserHostAddress;
            string userAgent = Request.Headers.UserAgent.ToString();
            //string time = Request.Headers.Date.ToString();
            long ticks = 321541646584;
            string token = GenerateToken(email, pswrd, ip, userAgent, ticks);
            return token;
           // return "IP "+ ip +" user agent: " + userAgent + " Date: " + time;
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

        private const int _expirationMinutes = 10000;
        public static bool IsTokenValid(string token, string ip, string userAgent)
        {
            bool result = false;
            try
            {
                // Base64 decode the string, obtaining the token:username:timeStamp.
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                // Split the parts.
                string[] parts = key.Split(new char[] { ':' });
                if (parts.Length == 3)
                {
                    // Get the hash message, username, and timestamp.
                    string hash = parts[0];
                    string username = parts[1];
                    long ticks = long.Parse(parts[2]);
                    DateTime timeStamp = new DateTime(ticks);
                    // Ensure the timestamp is valid.
                    bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalMinutes) > _expirationMinutes;
                    if (!expired)
                    {
                        //
                        // Lookup the user's account from the db.
                        //
                        if (username == "john")
                        {
                            string password = "password";
                            // Hash the message with the key to generate a token.
                            string computedToken = GenerateToken(username, password, ip, userAgent, ticks);
                            // Compare the computed token with the one supplied and ensure they match.
                            result = (token == computedToken);
                        }
                    }
                }
            }
            catch
            {
            }
            return result;
        }
    }
}
